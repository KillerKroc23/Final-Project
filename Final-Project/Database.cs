using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    class Database
    {
        private string conString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public List<Contact> GetContactList()
        {
            List<Contact> contactList = new List<Contact>();

            using (var con = new SqlConnection(conString))
            {
                SqlCommand cm = new SqlCommand("SELECT * from Manager", con);

                con.Open();

                using (SqlDataReader sdr = cm.ExecuteReader())
                {
                    if (!sdr.HasRows)
                    {
                        Console.WriteLine("There is an error. No contacts have been found.");
                    }
                    else
                    {
                        while (sdr.Read())
                        {
                            Contact firstContact = new Contact((int)sdr["ID"], (string)sdr["FirstName"], (string)sdr["LastName"], (string)sdr["Email"], (string)sdr["PhoneNumber"]);
                            contactList.Add(firstContact);
                        }
                    }
                }
            }
            return contactList;
        }

        public Contact GetContact(int ID)
        {
            Contact cont = null;

            using (var con = new SqlConnection(conString))
            {
                SqlCommand cm = new SqlCommand("SELECT * from Manager WHERE ID= @ID", con);
                cm.Parameters.AddWithValue("@ID", ID);
                con.Open();

                using (SqlDataReader sdr = cm.ExecuteReader())
                {
                    if (!sdr.HasRows)
                    {
                        Console.WriteLine("There is an error. No contacts have been found.");
                    }
                    else
                    {
                        sdr.Read();
                        cont = new Contact((int)sdr["ID"], (string)sdr["FirstName"], (string)sdr["LastName"], (string)sdr["Email"], (string)sdr["PhoneNumber"]);
                    }
                }
            }
            return cont;
        }
        
        public int AddContact(Contact ContactInfo)
        {
            int newID = 0;

            SqlConnection con = new SqlConnection(conString);

            string newQuery = "INSERT INTO Contacts (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";

            SqlCommand cm = new SqlCommand(newQuery, con);

            cm.Parameters.AddWithValue("@FirstName", ContactInfo.FirstName);
            cm.Parameters.AddWithValue("@LastName", ContactInfo.LastName);
            cm.Parameters.AddWithValue("@Email", ContactInfo.Email);
            cm.Parameters.AddWithValue("@PhoneNumber", ContactInfo.PhoneNumber);


            try
            {
                con.Open();

                var rowsAffected = cm.ExecuteNonQuery();
                Console.WriteLine("The contact has been created.");

                string newQuery2 = "SELECT @Identity as newID from Manager";
                cm.CommandText = newQuery2;
                cm.CommandType = System.Data.CommandType.Text;
                cm.Connection = con;

                newID = Convert.ToInt32(cm.ExecuteScalar());
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error has been found. The following is the error: " + e.ToString());
            }
            finally
            {
                con.Close();
            }
            return newID;
        }

        public void UpdateContact(Contact ContactInfo)
        {
            SqlConnection con = new SqlConnection(conString);
            string newQuery = "UPDATE Manager set FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber where ID = @ID";

            SqlCommand cm = new SqlCommand(newQuery, con);

            cm.Parameters.AddWithValue("@ID", ContactInfo.ID);
            cm.Parameters.AddWithValue("@FirstName", ContactInfo.FirstName);
            cm.Parameters.AddWithValue("@LastName", ContactInfo.LastName);
            cm.Parameters.AddWithValue("@Email", ContactInfo.Email);
            cm.Parameters.AddWithValue("@PhoneNumber", ContactInfo.PhoneNumber);

            try
            {
                con.Open();
                var rowsAffected = cm.ExecuteNonQuery();
                Console.WriteLine("The contact has been updated.");
                Console.WriteLine();
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error has been found. The following is the error: " + e.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public void DeleteContact(Contact ContactInfo)
        {
            SqlConnection con = new SqlConnection(conString);
            string newQuery = "DELETE from Manager where ID = @ID0";
            SqlCommand cm = new SqlCommand(newQuery, con);

            cm.Parameters.AddWithValue("@ID", ContactInfo.ID);

            try
            {
                con.Open();

                var rowsAffected = cm.ExecuteNonQuery();
                Console.WriteLine("The contact has been deleted.");
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error has been found. The following is the error: " + e.ToString());
            }
            finally
            {
                con.Close();
            }
        }  
    }
}
