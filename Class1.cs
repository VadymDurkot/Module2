using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WindowsFormsApp1
{
    [Serializable]
    class Zavod
    {
        public string Surname { get; set; }
        public int Id { get; set; }
        public string Position { get; set; }
        public int Experience { get; set; }
        public int Salary { get; set; }
        

        public Zavod(string surname, int id, string position, int experience, int salary)
        {
            Surname = surname;
            Id = id;
            Position = position;
            Experience = experience;
            Salary = salary;

        }
        public override string ToString()
        {
            return $"Surname:{Surname}\n Id:{Id} \n Position{Position}\n Experience{Experience} \n Salary{Salary}";
        }
        // returns Xml based on object
        public XmlElement ToXmlElement(XmlDocument doc)
        {
            XmlElement gallery = doc.CreateElement("gallery");
            gallery.InnerText = Surname;
            gallery.SetAttribute("id", Id.ToString());
            gallery.SetAttribute("position", Position);
            gallery.SetAttribute("experience", Experience.ToString().ToLower());
            gallery.SetAttribute("salary", Salary.ToString());

            return gallery;
        }
        // returns Object based on Xml
        public static Zavod FromXmlElement(XmlElement element)
        {
            int Id = Convert.ToInt32(element.GetAttribute("id"));
            string Surname = element.InnerText;
            string Position = element.GetAttribute("position");
            int Experience = Convert.ToInt32(element.GetAttribute("experience"));
            int Salary = Convert.ToInt32(element.GetAttribute("salary"));
            return new Zavod(Surname, Id, Position, Experience, Salary);
        }
    }
}
