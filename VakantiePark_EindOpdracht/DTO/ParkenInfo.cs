using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieParkBL.Model;

namespace VakantieParkBL.DTO
{
    public class ParkenInfo
    {
        public ParkenInfo(List<string> locaties, List<string> faciliteiten) 
        {
            locaties.Insert(0,"Alle Parken");
            Locaties = locaties;
            Faciliteiten = faciliteiten;
        }

        public List<string> Locaties { get; private set; } = new List<string>();
        public List<string> Faciliteiten { get; private set;} = new List<string>();

    }
}
