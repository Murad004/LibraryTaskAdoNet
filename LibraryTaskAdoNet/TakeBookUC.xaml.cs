using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryTaskAdoNet
{
    /// <summary>
    /// Interaction logic for TakeBookUC.xaml
    /// </summary>
    public partial class TakeBookUC : UserControl
    {
        SqlConnection conn;
        string cs = "";
        DataTable table;
        SqlDataReader reader;
        DataRowView DataRowView;
        int id;
        string name;
        int pages;
        int yearpress;
        int Id_Themes;
        int Id_Category;
        int Id_Author;
        int Id_Press;
        string comment;
        int quantity;
        int idcard;
        int studentcard;
        int bookcard;
        DateTime Dateout;
        DateTime Datein;
        int libcard;
        int count;
        public TakeBookUC()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (conn = new SqlConnection())
            {
                conn.ConnectionString = cs;
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from Books";
                command.Connection = conn;
                table = new DataTable();

                bool hasColumnAdded = false;
                using (reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (!hasColumnAdded)
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            hasColumnAdded = true;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                    MyDataGrid.ItemsSource = table.DefaultView;

                }



            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView = MyDataGrid.SelectedItem as DataRowView;
            try
            {
                if (DataRowView != null)
                {
                    id = int.Parse(DataRowView["Id"].ToString());
                    name = DataRowView["Name"].ToString();
                    pages = int.Parse(DataRowView["Pages"].ToString());
                    yearpress = int.Parse(DataRowView["YearPress"].ToString());
                    Id_Themes = int.Parse(DataRowView["Id_Themes"].ToString());
                    Id_Category = int.Parse(DataRowView["Id_Category"].ToString());
                    Id_Author = int.Parse(DataRowView["Id_Author"].ToString());
                    Id_Press = int.Parse(DataRowView["Id_Press"].ToString());
                    comment = DataRowView["Comment"].ToString();
                    quantity = int.Parse(DataRowView["Quantity"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlTransaction transaction = null;
            using (conn = new SqlConnection())
            {
                conn.ConnectionString = cs;
                conn.Open();
                transaction = conn.BeginTransaction();
                ++count;
                SqlCommand command = new SqlCommand("sp_TakeBook", conn);
                command.CommandType = CommandType.StoredProcedure;





                var param1 = new SqlParameter();
                param1.Value = FirstNameTxtBox.Text.ToString();
                param1.ParameterName = "@FirstName";
                param1.SqlDbType = SqlDbType.NVarChar;
                command.Parameters.Add(param1);





                var param2 = new SqlParameter();
                param2.Value = PasswordTxtBox.Password.ToString();
                param2.ParameterName = "@Password";
                param2.SqlDbType = SqlDbType.NVarChar;
                command.Parameters.Add(param2);
                command.Transaction = transaction;



                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    MyGrid.Children.Add(TakeBookUC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    conn.Close();
                }
            }
        }
    }
}
