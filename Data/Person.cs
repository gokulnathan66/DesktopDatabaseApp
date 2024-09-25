using System;


namespace FinalProject
{
    public class Person

    {
    
        public string? Name { get; set; }

        public string? Particular { get; set; }
        public int Quantity { get; set; }
        
        public int BasicRate { get; set; }
        public int Value { get; set; }
        public int AdvancePaid { get; set; }
        public DateTime TransactionDate { get; internal set; }
    }

}
