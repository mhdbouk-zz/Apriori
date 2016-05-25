using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;

namespace Apriori
{
    public partial class TableUserControl : UserControl
    {
        public TableUserControl(ItemSet itemSet, List<AssociationRule> rules)
        {
            InitializeComponent();
            ItemSetLabel.Text = itemSet.Label;
            foreach (var item in itemSet)
            {
                ItemSetsDataGridView.Rows.Add(item.Key.ToDisplay(), item.Value);
                if (item.Value < itemSet.Support)
                    ItemSetsDataGridView.Rows[ItemSetsDataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            }
            if (rules.Count == 0)
            {
                ItemSetsDataGridView.Height = 342;
                RulesDataGridView.Hide();
            }
            else
            {
                foreach (var item in rules)
                {
                    RulesDataGridView.Rows.Add(item.Label, item.Confidance.ToPercentString(), item.Support.ToPercentString());
                }
            }
        }

        public TableUserControl(List<string> Values)
        {
            InitializeComponent();
            ItemSetLabel.Text = "Transactions";
            ItemSetsDataGridView.Columns[0].Name = "TransactionID";
            ItemSetsDataGridView.Columns[1].Name = "Items";
            for (int i = 0; i < Values.Count; i++)
            {
                ItemSetsDataGridView.Rows.Add(i, Values[i]);
            }
            ItemSetsDataGridView.Height = 342;
            RulesDataGridView.Hide();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ItemSetsDataGridView.ClearSelection();
        }
    }
}
