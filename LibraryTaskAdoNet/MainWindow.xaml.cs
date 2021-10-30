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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        string cs = "";
        int count = 0;
        TakeBookUC TakeBookUC = new TakeBookUC();
        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlTransaction transaction = null;
            using (conn = new SqlConnection())
            {
                conn.ConnectionString = cs;
                conn.Open();
                transaction = conn.BeginTransaction();
                ++count;
                SqlCommand command = new SqlCommand("sp_SignIn", conn);
                command.CommandType = CommandType.StoredProcedure;





                var param1 = new SqlParameter();
                param1.Value = FirstNameTxtBox.Text.ToString();
                param1.ParameterName = "@FirstName";
                param1.SqlDbType = SqlDbType.NVarChar;
                command.Parameters.Add(param1);





                var param2 = new SqlParameter();
                param2.Value = PassBox.Password.ToString();
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

        private void SignInBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
