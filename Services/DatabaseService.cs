using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
namespace FinalProject
{


    public class DatabaseService
    {
        public SQLiteConnection _connection;

        public DatabaseService(string databasePath)
        {

            _connection = new SQLiteConnection($"Data Source={databasePath}");
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile($"{ databasePath}");
                Console.WriteLine("database is created");
                
            }
            CreateTables();
        }
        private void CreateTables()
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    // Create SupplierTransactions table if it doesn't exist
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS SupplierTransactions (
                            TransactionID INTEGER PRIMARY KEY AUTOINCREMENT,
                            TransactionDate TEXT NOT NULL,
                            SupplierName TEXT NOT NULL,
                            Particulars TEXT NOT NULL,
                            Quantity INTEGER NOT NULL,
                            BasicRate INTEGER NOT NULL,
                            ValueINR INTEGER NOT NULL,
                            AdvancePaidINR INTEGER NOT NULL
                        );";
                    command.ExecuteNonQuery();

                    // Create supplier_list table if it doesn't exist
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS supplier_list (
                            supplier_id INTEGER PRIMARY KEY AUTOINCREMENT,
                            supplier_name TEXT NOT NULL
                        );";
                    command.ExecuteNonQuery();

                    // Create leaf_list table if it doesn't exist
                    command.CommandText = @"
                              CREATE TABLE IF NOT EXISTS leaf_list (
                            leaf_id INTEGER PRIMARY KEY AUTOINCREMENT,
                            leaf_name TEXT NOT NULL,
                            base_rate INTEGER NOT NULL,
                            value INTEGER NOT NULL
                        );";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating tables: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void AddPerson(Person person)
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }
                using (var command = _connection.CreateCommand())
                {
                    // Insert the new record into SupplierTransactions table
                    command.CommandText = @"
                        INSERT INTO SupplierTransactions 
                        (TransactionDate,SupplierName, Particulars, Quantity, BasicRate, ValueINR, AdvancePaidINR)
                        VALUES (@Transactiondate,@name, @particular, @quantity, @basicRate, @value, @advancePaid)";

                    // Bind parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@Transactiondate", person.TransactionDate);
                    command.Parameters.AddWithValue("@name", person.Name);
                    command.Parameters.AddWithValue("@particular", person.Particular);
                    command.Parameters.AddWithValue("@quantity", person.Quantity);
                    command.Parameters.AddWithValue("@basicRate", person.BasicRate);
                    command.Parameters.AddWithValue("@value", person.Value);
                    command.Parameters.AddWithValue("@advancePaid", person.AdvancePaid);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting person into database: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<Person> GetPeople(string year, string supplier)
        {
            List<Person> people = new List<Person>();

            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    // Updated query to filter by year and supplier
                    command.CommandText = @"
                SELECT SupplierName, Particulars, Quantity, BasicRate, ValueINR, AdvancePaidINR 
                FROM SupplierTransactions 
                WHERE SupplierName = @supplier
                AND strftime('%Y', TransactionDate) = @year"; 

                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@supplier", supplier);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            people.Add(new Person
                            {
                                Name = reader.GetString(0),
                                Particular = reader.GetString(1),
                                Quantity = reader.GetInt32(2),
                                BasicRate = reader.GetInt32(3),
                                Value = reader.GetInt32(4),
                                AdvancePaid = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving people from database: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return people;
        }
        public ObservableCollection<string> GetSupplier_list()
        {
            ObservableCollection<string> names = new ObservableCollection<string>();

            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    // Query to select all supplier names
                    command.CommandText = @"
                SELECT supplier_name
                FROM supplier_list";

                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through the results and add supplier names to the list
                        while (reader.Read())
                        {
                            names.Add(reader.GetString(0));  // Add the supplier name to the list
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving supplier names from database: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return names;
        }
        public List<Person> GetMontylyReport(string year, string month)
        {
            List<Person> people = new List<Person>();

            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    // SQL query to select data based on year and month
                    command.CommandText = @"
            SELECT SupplierName, Particulars, Quantity, BasicRate, ValueINR, AdvancePaidINR
            FROM SupplierTransactions
            WHERE strftime('%Y', TransactionDate) = @year
            AND strftime('%m', TransactionDate) = @month";

                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@month", month);

                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through the results and add each person to the list
                        while (reader.Read())
                        {
                            people.Add(new Person
                            {
                                Name = reader.GetString(0),        // Supplier Name
                                Particular = reader.GetString(1),  // Particulars
                                Quantity = reader.GetInt32(2),     // Quantity
                                BasicRate = reader.GetInt32(3),    // Basic Rate
                                Value = reader.GetInt32(4),        // Value in INR
                                AdvancePaid = reader.GetInt32(5)   // Advance Paid in INR
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving data from the database: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return people;
        }
        public void AddSupplier(string supplierName)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = @"
            INSERT INTO supplier_list (supplier_name)
            VALUES (@supplierName)";

                    command.Parameters.AddWithValue("@supplierName", supplierName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding supplier: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void UpdateSupplier(string oldSupplierName, string newSupplierName)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = @"
            UPDATE supplier_list
            SET supplier_name = @newSupplierName
            WHERE supplier_name = @oldSupplierName";

                    command.Parameters.AddWithValue("@newSupplierName", newSupplierName);
                    command.Parameters.AddWithValue("@oldSupplierName", oldSupplierName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating supplier: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void DeleteSupplier(string supplierName)
        {
            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = @"
            DELETE FROM supplier_list
            WHERE supplier_name = @supplierName";

                    command.Parameters.AddWithValue("@supplierName", supplierName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting supplier: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }


        public List<string> GetLeafList()
        {
            List<string> leafNames = new List<string>();

            try
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    // Query to select all leaf names
                    command.CommandText = @"
                SELECT leaf_name
                FROM leaf_list";

                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through the results and add leaf names to the list
                        while (reader.Read())
                        {
                            leafNames.Add(reader.GetString(0));  // Add the leaf name to the list
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving leaf names from database: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return leafNames;
        }

        public void AddLeaf(string leafName, int baseRate, int value)
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO leaf_list (leaf_name, base_rate, value)
                    VALUES (@leafName, @baseRate, @value);";
                command.Parameters.AddWithValue("@leafName", leafName);
                command.Parameters.AddWithValue("@baseRate", baseRate);
                command.Parameters.AddWithValue("@value", value);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }


        public void UpdateLeaf(string oldLeafName, string newLeafName, int baseRate, int value)
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"
                    UPDATE leaf_list 
                    SET leaf_name = @newLeafName, base_rate = @baseRate, value = @value
                    WHERE leaf_name = @oldLeafName;";
                command.Parameters.AddWithValue("@newLeafName", newLeafName);
                command.Parameters.AddWithValue("@baseRate", baseRate);
                command.Parameters.AddWithValue("@value", value);
                command.Parameters.AddWithValue("@oldLeafName", oldLeafName);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void DeleteLeaf(string leafName)
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM leaf_list WHERE leaf_name = @leafName;";
                command.Parameters.AddWithValue("@leafName", leafName);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        public (int BaseRate, int Value) GetLeafDetails(string leafName)
        {
            {
                _connection.Open();

                // Define the SQL query to retrieve base_rate and value for the specified leaf_name
                var command = _connection.CreateCommand();
                command.CommandText = @"
            SELECT base_rate, value
            FROM leaf_list
            WHERE leaf_name = @leafName;
        ";

                // Add parameter to prevent SQL injection
                command.Parameters.AddWithValue("@leafName", leafName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Read the base_rate and value from the result
                        var baseRate = reader.GetInt16(0); // base_rate
                        var value = reader.GetInt16(0); ;    // value
                        return (baseRate, value);
                    }
                    else
                    {
                        // If no record is found, return default values
                        return (0, 0);
                    }
                }
            }
        }

    }
}