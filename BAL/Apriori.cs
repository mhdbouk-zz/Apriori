using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class Apriori
    {
        string _FilePath;
        List<string> list;
        List<string> DistinctValues;
        List<ItemSet> ItemSets;
        public Apriori(string FilePath)
        {
            _FilePath = FilePath;
            list = File.ReadAllLines(FilePath).ToList().Where(a=>!string.IsNullOrWhiteSpace(a)).ToList();
            ItemSets = new List<ItemSet>();
            SetDistinctValues(list);
        }

        public ItemSet GetItemSet(int length, int support, bool Candidates = false, bool IsFirstItemList = false)
        {
            List<IEnumerable<string>> result = GetPermutations(DistinctValues, length).ToList();
            List<List<string>> data = new List<List<string>>();
            foreach (var item in result)
            {
                data.Add(item.ToList());
            }
            ItemSet itemSet = new ItemSet();
            itemSet.Support = support;
            itemSet.Label = (Candidates ? "C" : "L") + length.ToString();
            foreach (var item in data)
            {
                int count = 0;
                foreach (var word in list)
                {
                    bool found = false;
                    foreach (var item2 in item)
                    {
                        if (word.Split(' ').Contains(item2))
                            found = true;
                        else
                        {
                            found = false;
                            break;
                        }

                    }
                    if (found)
                        count++;
                }
                if ((Candidates && count > 0) || IsFirstItemList || count >= support)
                {
                    itemSet.Add(item, count);
                    ItemSets.Add(itemSet);
                }
            }
            return itemSet;
        }

        public void SetDistinctValues(List<string> values)
        {
            List<string> data = new List<string>();
            foreach (var item in values)
            {
                var row = item.Split(' ');
                foreach (var item2 in row)
                {
                    if (string.IsNullOrWhiteSpace(item2)) continue;
                    if (!data.Contains(item2))
                        data.Add(item2);
                }
            }
            DistinctValues = new List<string>();
            DistinctValues.AddRange(data.OrderBy(a => a).ToList());
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

        public List<AssociationRule> GetRules(ItemSet items)
        {
            List<AssociationRule> rules = new List<AssociationRule>();
            foreach (var item in items)
            {
                foreach (var set in item.Key)
                {
                    rules.Add(GetSingleRule(set, item));
                    if (item.Key.Count > 2)
                        rules.Add(GetSingleRule(item.Key.ToDisplay(exclude: set), item));
                }
            }

            return rules.OrderByDescending(a => a.Support).ThenByDescending(a => a.Confidance).ToList();
        }

        private AssociationRule GetSingleRule(string set, KeyValuePair<List<string>, int> item)
        {
            var setItems = set.Split(',');
            for (int i = 0; i < setItems.Count(); i++)
            {
                setItems[i] = setItems[i].Trim();
            }
            AssociationRule rule = new AssociationRule();
            StringBuilder sb = new StringBuilder();
            sb.Append(set).Append(" => ");
            List<string> list = new List<string>();
            foreach (var set2 in item.Key)
            {
                if (setItems.Contains(set2)) continue;
                list.Add(set2);
            }
            sb.Append(list.ToDisplay());
            rule.Label = sb.ToString();
            int totalSet = 0;
            foreach (var first in ItemSets)
            {
                var myItem = first.Keys.Where(a => a.ToDisplay() == set);
                if (myItem.Count() > 0)
                {
                    first.TryGetValue(myItem.FirstOrDefault(), out totalSet);
                    break;
                }
            }
            rule.Confidance = Math.Round(((double)item.Value / totalSet) * 100, 2);
            rule.Support = Math.Round(((double)item.Value / this.list.Count) * 100, 2);
            return rule;
        }
    }
}