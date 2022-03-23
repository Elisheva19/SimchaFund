using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimchaFund.Data
{
    public class Manager
    {

        public string _connection;
        public Manager(string connect)
        {
            _connection = connect;
        }

        public List<Simcha> GetSimchas(int simchamount)
        {

            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select s.*, COUNT(c.amount) as count, SUM(c.amount) as total
                                from Simchas s  left join Contributions c
                                on s.Id =c.SimchaId
                                group by s.Id,s.Name, s.Date";
            conn.Open();
            var result = new List<Simcha>();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["name"],
                    Date = (DateTime)reader["date"],
                    Count = (int?)reader["count"],
                    Total = reader.GetOrNull<decimal?>("total")
                });


            }
            return result;
        }

        public int GetContributorAmount()
        {

            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select count(*) 
                from contributors c";
            conn.Open();
            int result = (int)cmd.ExecuteScalar();
            return result;
        }


        public int AddSimcha(string name, DateTime date)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into simchas
                                values(@name,@date) select scope_identity()";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@date", date);
            conn.Open();

            int id = (int)(decimal)cmd.ExecuteScalar();
            return id;
        }

        public List<Contributor> GetContributors()
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from contributors";
            conn.Open();
            var result = new List<Contributor>();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"],
                    DateCreated = (DateTime)reader["dateCreated"],
                    CellNumber = (string)reader["cellnumber"],
                    AlwaysInclude = (bool)reader["alwaysinclude"]
                });


            }
            return result;
        }

        public decimal GetDeposit(int id)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select isnull (sum(d.amount),0) as 'deposits' from Deposits d 
                            where d.Personid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            decimal sum = (decimal)cmd.ExecuteScalar();

            return sum;
        }
        public decimal Getwithdrawals(int id)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select ISNULL (sum(c.amount),0)  as 'withdrawals' from contributions c 
                            where c.Personid =@id";

            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            decimal sum = (decimal)cmd.ExecuteScalar();

            return sum;
        }
        public decimal GetAmount(int id, int simchaid)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select ISNULL  (  sum(ct.Amount),0) 
                                from Contributions ct
                                where ct.PersonId=@id and CT.SimchaId=@simchaid";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@simchaid", simchaid);
            conn.Open();
            decimal sum = (decimal)cmd.ExecuteScalar();

            return sum;
        }

        public void AddDeposit(Deposit newdeposit)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into Deposits
                            values(@id, @amount, @date)";
            cmd.Parameters.AddWithValue("@id", newdeposit.PersonId);
            cmd.Parameters.AddWithValue("@amount", newdeposit.Amount);
            cmd.Parameters.AddWithValue("@date", newdeposit.Date);
            conn.Open();
            cmd.ExecuteNonQuery();

        }

        public int AddContributor(Contributor newcontri)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into Contributors
                           values(@firstname, @lastname, @cellnumber, @date, @include) select scope_identity()";
            cmd.Parameters.AddWithValue("@firstname", newcontri.FirstName);
            cmd.Parameters.AddWithValue("@lastname", newcontri.LastName);
            cmd.Parameters.AddWithValue("@cellnumber", newcontri.CellNumber);
            cmd.Parameters.AddWithValue("@date", newcontri.DateCreated);
            cmd.Parameters.AddWithValue("@include", newcontri.AlwaysInclude);
            conn.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();

            return id;


        }
        public void UpdateContributor(Contributor update)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"update Contributors
                           set FirstName = @firstname, LastName = @lastname, CellNumber = @cellnumber, DateCreated = @date, AlwaysInclude = @include where Id = @Id";
            cmd.Parameters.AddWithValue("@firstname", update.FirstName);
            cmd.Parameters.AddWithValue("@lastname", update.LastName);
            cmd.Parameters.AddWithValue("@cellnumber", update.CellNumber);
            cmd.Parameters.AddWithValue("@date", update.DateCreated);
            cmd.Parameters.AddWithValue("@include", update.AlwaysInclude);
            cmd.Parameters.AddWithValue("@Id", update.Id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public void DeleteForSimcha(int simchaid)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"delete from contributions where SimchaId= @Id";
            cmd.Parameters.AddWithValue("@Id", simchaid);
            conn.Open();
            cmd.ExecuteNonQuery();

        }

        public void AddContribution(Contribution addmoney)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into Contributions
                           values(@personid, @simchaid, @amount)";
            cmd.Parameters.AddWithValue("@personid", addmoney.PersonId);
            cmd.Parameters.AddWithValue("@simchaid", addmoney.SimchaId);
            cmd.Parameters.AddWithValue("@amount", addmoney.Amount);

            conn.Open();
            cmd.ExecuteNonQuery();

        }
        public string GetSimchaName( int id)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select s.name from Simchas s where
                s.Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            
            conn.Open();
          string simname= (string)cmd.ExecuteScalar();
            return simname;

        }
        public string GetContributorName(int id)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select c.Firstname, c.LastName from Contributors  c
                                where c.Id=@id";
            cmd.Parameters.AddWithValue("@id", id);

           
            var result = " ";
            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result = (string)reader["FirstName"];
                result += " ";
                 result+= reader["LastName"];
               
};

            return result;
        }

                
                



        public List<HistoryTrans> History(int contribid)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"
                           select s.name, s.date, ct.amount
                            from Simchas s join Contributions ct on s.Id=ct.SimchaId
                             where ct.PersonId=@id";

            cmd.Parameters.AddWithValue("@id", contribid);
            var result = new List<HistoryTrans>();
            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new HistoryTrans
                {

                    SimchaName = (string)reader["name"],
                    Date = (DateTime)reader["date"],
                    Amount = (decimal)reader["amount"]

                });


            }

            return result;

        }
        public List<HistoryTrans> GetHistoryDeposits(int id, List<HistoryTrans> result)
        {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @" select * from deposits 
                       where Personid=@id";

            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new HistoryTrans
                {

                    SimchaName = "Deposit",
                    Date = (DateTime)reader["date"],
                    Amount = -(decimal)reader["amount"]

                });


            }

            return result;
        }

    }
    }

public static class Extensions
{
    public static T GetOrNull<T>(this SqlDataReader reader, string columnName)
    {
        object value = reader[columnName];
        if (value == DBNull.Value)
        {
            return default(T);
        }

        return (T)value;
    }
}