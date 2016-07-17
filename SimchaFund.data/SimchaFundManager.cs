using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.data
{
    public class SimchaFundManager
    {
        private string _connectionString;
        public SimchaFundManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddContributor(string name, string phone, int include)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Contributors(Name,Phone,AlwaysInclude) "
                    + "VALUES(@name,@phone,@include) SELECT @@identity";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@include", include);
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }

        public void AddDeposit(int amount, DateTime date, int contributorId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Deposits(Amount,DepositDate,ContributorId) "
                    + "VALUES(@amount,@date,@contributorId)";
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Contributor> GetallContributors()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Contributors";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Contributor> contributors = new List<Contributor>();
                while (reader.Read())
                {
                    contributors.Add(new Contributor
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        PhoneNumber = (string)reader["Phone"],
                        AlwaysInclude = (bool)reader["AlwaysInclude"]
                    });
                }
                return contributors;
            }
        }

        public Contributor GetContributorById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Contributors WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Contributor contributor = new Contributor
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    PhoneNumber = (string)reader["Phone"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"]
                };
                return contributor;
            }
        }

        public void EditContributor(int id, string name, string phone, int include)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Contributors SET Name = @name,Phone = @phone, AlwaysInclude = @include "
                    + "WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@include", include);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ContributionForSimcha> NamesOfContributorsForSimcha(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT c.Name,co.ContributorId,co.Amount FROM Contributors c JOIN Contributions co "
                    + " ON c.Id = co.ContributorId  WHERE co.SimchaId = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<ContributionForSimcha> contributors = new List<ContributionForSimcha>();
                while (reader.Read())
                {
                    contributors.Add(new ContributionForSimcha
                    {
                        Name = (string)reader["Name"],
                        ContributorId = (int)reader["ContributorId"],
                        Amount = (decimal)reader["Amount"]
                    });
                }
                return contributors;
            }
        }

        public decimal SumOfDeposits(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SUM(Amount) FROM Deposits";
                if (id > 0)
                {
                    command.CommandText += " WHERE ContributorId = @id";
                    command.Parameters.AddWithValue("@id", id);
                }
                connection.Open();
                if (CountOfDeposits() > 0)
                {
                    return (decimal)command.ExecuteScalar();
                }
                return 0;
            }
        }

        public IEnumerable<Simcha> GetAllSimchos()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Simchas";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Simcha> simchos = new List<Simcha>();
                while (reader.Read())
                {
                    simchos.Add(new Simcha
                    {
                        Id = (int)reader["Id"],
                        BalSimchaName = (string)reader["SimchaName"],
                        SimchaDate = (DateTime)reader["Date"]
                    });
                }
                return simchos;
            }
        }

        public int AddSimcha(string name, DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Simchas(SimchaName,Date) "
                    + "VALUES(@name,@date) SELECT @@IDENTITY";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@date", date);
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }

        public Simcha GetSimchaById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Simchas WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Simcha simcha = new Simcha
                {
                    Id = (int)reader["Id"],
                    BalSimchaName = (string)reader["SimchaName"],
                    SimchaDate = (DateTime)reader["Date"]
                };
                return simcha;
            }
        }

        public void AddContributionsForSimcha(List<ContributionForSimcha> contributions)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();                
                foreach (ContributionForSimcha c in contributions)
                {
                    SqlCommand command = connection.CreateCommand();
                    if (c.Contribute)
                    {

                        if (CountOfContributionsForSimchaForPerson(c) > 0)
                        {
                            command.CommandText = "UPDATE Contributions SET Amount = @amount "
                                + "WHERE ContributorId = @cId AND SimchaId = @sId";
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Contributions(ContributorId,SimchaId,Amount) "
                                + "VALUES(@cId,@sId,@amount)";
                        }
                    }
                    else 
                    {
                        command.CommandText = "DELETE Contributions WHERE ContributorId = @cId AND SimchaId = @sId";
                    }
                    command.Parameters.AddWithValue("@cId", c.ContributorId);
                    command.Parameters.AddWithValue("@sId", c.SimchaId);
                    command.Parameters.AddWithValue("@amount", c.Amount);
                    command.ExecuteNonQuery();
                }
            }
        }

        public decimal SumOfContributions(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SUM(Amount) FROM Contributions";
                if (id > 0)
                {
                    command.CommandText += " WHERE ContributorId = @id";
                    command.Parameters.AddWithValue("@id", id);
                }
                connection.Open();
                if (CountOfContributions(id) > 0)
                {
                    return (decimal)command.ExecuteScalar();
                }
                return 0;
            }
        }

        public decimal SumOfContributionsForSimcha(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SUM(Amount) FROM Contributions WHERE SimchaId = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                if (CountOfContributionsForSimcha(id) > 0)
                {
                    return (decimal)command.ExecuteScalar();
                }
                return 0;
            }
        }

        public IEnumerable<Deposit> GetAllDeposits(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Deposits WHERE ContributorId = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Deposit> deposits = new List<Deposit>();
                while (reader.Read())
                {
                    deposits.Add(new Deposit
                    {
                        Id = (int)reader["Id"],
                        Amount = (decimal)reader["Amount"],
                        DepositDate = (DateTime)reader["DepositDate"],
                        ContributorId = (int)reader["ContributorId"]
                    });
                }
                return deposits;
            }
        }

        public IEnumerable<Contribution> GetAllContributions(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT s.SimchaName,c.Amount,s.Date FROM Simchas s "
                    + "JOIN Contributions c  ON s.Id = c.SimchaId WHERE c.ContributorId = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Contribution> contributions = new List<Contribution>();
                while (reader.Read())
                {
                    contributions.Add(new Contribution
                    {
                        SimchaName = (string)reader["SimchaName"],
                        Amount = (decimal)reader["Amount"],
                        Date = (DateTime)reader["Date"]
                    });
                }
                return contributions;
            }
        }

        public int CountOfContributors()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Contributors";
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public int CountOfContributionsForSimcha(int simchaId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Contributions WHERE SimchaId = @simchaId";
                command.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        private int CountOfContributions(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(Amount) FROM Contributions"; 
                 if (id > 0)
                {
                    command.CommandText += " WHERE ContributorId = @id";
                    command.Parameters.AddWithValue("@id", id);
                }
                connection.Open();
                return (int)command.ExecuteScalar();

            }
        }

        private int CountOfDeposits()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(Amount) FROM Deposits";
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        private int CountOfContributionsForSimchaForPerson(ContributionForSimcha c)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(Amount) FROM Contributions WHERE ContributorId = @cId AND SimchaId = @sID";
                command.Parameters.AddWithValue("@cid", c.ContributorId);
                command.Parameters.AddWithValue("@sid", c.SimchaId);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
    }
}
