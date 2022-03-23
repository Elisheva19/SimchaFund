using System;

namespace SimchaFund.Data
{
}
    public class Contributor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool AlwaysInclude { get; set; }
    public decimal? Balance { get; set; }
    public decimal Amount { get; set; }
    public bool Include { get; set; }
    

   

    }


    public class Simcha
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int? Count { get; set; }
        public decimal? Total { get; set; }
    }

    public class Contribution
    {
       
        public int PersonId { get; set; }
        public int SimchaId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Deposit
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public int PersonId { get; set; }
        public DateTime Date { get; set; }
    }
public class HistoryTrans
{
    public string SimchaName { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}