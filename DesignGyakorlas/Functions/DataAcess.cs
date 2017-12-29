using Dapper;
using DesignGyakorlas.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DesignGyakorlas.Functions
{
    public class DataAcess
    {
        IDbConnection testconnection;
        bool IsSuccesfullyConnected = false;

        public bool IsConnectionSuccesful { get { return IsSuccesfullyConnected; } }

        public DataAcess()
        {
            testconnection = new MySqlConnection(ConnectionString());

            Task.Factory.StartNew(delegate {
                try
                {
                    testconnection.Open();
                    IsSuccesfullyConnected = true;
                }
                catch (Exception ex)
                {
                    //  MessageBox.Show("Cant connect to the autocomplete database, autocomplete will not work");
                    IsSuccesfullyConnected = false;
                }
            });
         //   Task.WaitAll(new Task[1] {})
          
            
        }

       private static string ConnectionString()
        {
            //  return ConfigurationManager.ConnectionStrings[ConnName].ConnectionString;
            return "server=127.0.0.1;persistsecurityinfo=True;user id=root;database=designgyakorlasapp";
        }

        public List<ItemViewModel> GetSearchedItem(string itemName)
        {
            using (IDbConnection connection = new MySqlConnection(ConnectionString()))
            {
                if (IsSuccesfullyConnected)
                {
                    connection.Close();
                    connection.Open();
                    return connection.Query<ItemViewModel>($"select * from AutocompleteItems where ItemName = '{itemName}'").ToList();
                }
                else
                    return null;
            }
        }

        public List<ItemViewModel> GetSearchedItemWithQuery(string onlyQuery)
        {
            using (IDbConnection connection = new MySqlConnection(ConnectionString()))
            {
                if (IsSuccesfullyConnected)
                {
                    connection.Close();
                    connection.Open();
                    return connection.Query<ItemViewModel>($"{onlyQuery}").ToList();
                }
                else
                    return new List<ItemViewModel>();
            }
        }
       

        public void RunQuery(string onlyQuery)
        {
            using(IDbConnection connection = new MySqlConnection(ConnectionString()))
            {
                if (IsSuccesfullyConnected)
                {
                    connection.Open();
                    connection.Query(onlyQuery);
                }
            }
            
               
        }

    
    }
}
