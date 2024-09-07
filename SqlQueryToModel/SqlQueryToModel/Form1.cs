using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlQueryToModel
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string result = "";
            var query = txtSQLQuery.Text;

            query = query.ToLower();

            //Remove select
            query = query.Replace("select", "");

            //If contains from, then remove
            if (query.Contains("from"))
            {
                query = query.Substring(0,query.IndexOf("from"));
            }

            //Get the propertys
            var propertiesName = query.Split(',');

            foreach (var item in propertiesName)
            {
                string property = "",propName = item,dataType = "int";

                if (item.Contains("$"))
                {
                    dataType = item.Substring(item.IndexOf("$"), item.Length - item.IndexOf("$"));
                    dataType = dataType.Replace("$","");
                }

                //If contains alias, then get alias' name
                if (item.Contains(" as "))
                {
                    propName = item.Substring(item.IndexOf(" as "), item.Contains("$") ?
                        item.IndexOf("$") - item.IndexOf(" as ") :
                        item.Length - item.IndexOf(" as "));

                    propName = propName.Replace(" as ", "");
                }

                propName = propName.Trim();
                
                //Create the property with the correct name
                property = $"public {dataType} {propName} {{ get; set; }}";

                //Add to result
                result = $"{result}\n{property}";
            }

            txtResult.Text = result;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtResult.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSQLQuery.Text = "";
            txtResult.Text = "";
            txtSQLQuery.Focus();
        }
    }
}
