using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ICS.Core.Host.Data.Entities
{
    public class CredentialCard
    {
        public string Rfid { get; set; }
        [Key]
        public string CardNumber { get; set; }
        public Client Client { get; set; }
        public CredentialCardCondition Condition { get; set; }
        public CredentialCard()
        {
            Rfid = Guid.NewGuid().ToString();
            CardNumber = "";
            var number = Guid.NewGuid().ToString().ToCharArray().ToList();
            number.RemoveRange(18, 18);
            number.ForEach(x => CardNumber += x);
        }

        public OutcomeCredentialCard ToOutcome()
        {
            return new OutcomeCredentialCard()
            {
                Rfid = Rfid,
                CardNumber = CardNumber,
                Condition = Condition
            };
        }
    }
}
