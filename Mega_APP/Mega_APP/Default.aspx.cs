using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Mega_APP
{
    public partial class _Default : System.Web.UI.Page
    {
        const string myInitCatalog = "; Initial Catalog=";
        const string myQuery = "SELECT TOP 10 * from ";
        const string myDeleteQuery = "DROP table dbo.";
        const string myCreateQuery = "CREATE table dbo.";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                using (var aCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString()))
                {
                    int aCount = 0;
                    aCon.Open();
                    DataTable aDBs = aCon.GetSchema("Databases");
                    TreeNode aChild = new TreeNode
                    {
                        Text = "Databases",
                        Value = aCount.ToString()
                    };
                    TreeV.Nodes.Add(aChild);
                    TreeV.Attributes.Add("oncontextmenu", "ShowMenu('contextMenu',event)");
                    
                    foreach (DataRow aDBr in aDBs.Rows)
                    {
                        aCount++;
                        string aConnStr = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString() + myInitCatalog + aDBr.ItemArray[0].ToString();
                        PopulateTreeView(aDBr, aConnStr, aChild, aCount);
                    }
                }
            }
        }

        private void PopulateTreeView(DataRow theDBr, string theConnStr, TreeNode theChild, int tbeCount)
        {
            tbeCount *= 10;
            TreeNode aChild = new TreeNode
            {
                Text = theDBr.ItemArray[0].ToString(),
                Value = tbeCount.ToString()
            };
            theChild.ChildNodes.Add(aChild);

            using (var aCon = new SqlConnection(theConnStr))
            {
                TreeNode aCh;
                aCon.Open();
                DataTable aDBs = aCon.GetSchema("Tables");
                tbeCount *= 10;
                foreach (DataRow aDBr in aDBs.Rows)
                {
                    if (aDBr.ItemArray[3].ToString().Equals("BASE TABLE"))
                    {
                        tbeCount++;
                        aCh = new TreeNode
                        {
                            Text = aDBr.ItemArray[2].ToString(),
                            Value = tbeCount.ToString()
                        };
                        aChild.ChildNodes.Add(aCh);
                    }
                }
            }
        }

        private bool checkClickableNode(string theValue)
        {
            bool isResult = false;
            int aVl= 0;
            if (Int32.TryParse(theValue, out aVl))
            {
                if (aVl >= 100)
                {
                    isResult = true;
                }
            }
            return isResult;
        }

        protected void TreeV_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode aSNode = ((TreeView)sender).SelectedNode;
            if (checkClickableNode(aSNode.Value))
            {
                string aQuery = string.Empty;
                string anICatalog = string.Empty;
                if (aSNode.Parent != null)
                {
                    TreeNode aPNode = aSNode.Parent;
                    anICatalog = aPNode.Text;
                }
                aQuery = myQuery + aSNode.Text;
                DataTable dt = new DataTable();
                string dbConn = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString() + myInitCatalog + anICatalog;
                using (SqlConnection con = new SqlConnection(dbConn))
                {
                    using (SqlCommand cmd = new SqlCommand(aQuery))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                        }
                    }
                }
                GView.DataSource = dt;
                GView.DataBind();
            }
        }

        /// <summary>
        /// Функция для удаления таблицы из БД
        /// </summary>
        private void deleteTable(TreeNode theNode)
        {
            if (checkClickableNode(theNode.Value))
            {
                string aQuery = string.Empty;
                string anICatalog = string.Empty;
                if (theNode.Parent != null)
                {
                    TreeNode aPNode = theNode.Parent;
                    anICatalog = aPNode.Text;
                }
                aQuery = myDeleteQuery + theNode.Text;
                DataTable dt = new DataTable();
                string dbConn = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString() + myInitCatalog + anICatalog;
                using (SqlConnection con = new SqlConnection(dbConn))
                {
                    using (SqlCommand cmd = new SqlCommand(aQuery))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                        }
                    }
                }
                GView.DataSource = dt;
                GView.DataBind();
            }
        }

        /// <summary>
        /// Функция для создания таблицы в БД
        /// </summary>
        private void CreateTable(string theDBName, string theTbName)
        {
            string aQuery = myCreateQuery + theTbName;
            DataTable dt = new DataTable();
            string dbConn = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString() + myInitCatalog + theDBName;
            using (SqlConnection con = new SqlConnection(dbConn))
            {
                using (SqlCommand cmd = new SqlCommand(aQuery))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }
            GView.DataSource = dt;
            GView.DataBind();
        }
    }
}
