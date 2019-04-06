namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(e => ids.Contains(e.Id))
                .Select(e => new 
                {
                    Id = e.Id,
                    Name = e.FullName,
                    CellNumber = e.Cell.CellNumber,
                    Officers = e.PrisonerOfficers.Select(s => s.Officer).Select(n => new 
                    {
                        OfficerName = n.FullName,
                        Department = n.Department.Name
                    })
                    .OrderBy(p => p.OfficerName)
                    .ToList(),
                    TotalOfficerSalary = e.PrisonerOfficers.Select(s => s.Officer).Sum(pc => pc.Salary)
                })
                .OrderBy(p => p.Name)
                .ThenBy(g => g.Id)
                .ToList();

            var json = JsonConvert.SerializeObject(prisoners, new JsonSerializerSettings
            {
                //ContractResolver = contractResolver,
                Formatting = Newtonsoft.Json.Formatting.Indented,
            });

            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisoners = context.Prisoners
                .Where(e => prisonersNames.Contains(e.FullName))
                .OrderBy(e => e.FullName)
                .ThenBy(e => e.Id)
                 .Select(p => new ExportPrisonersDto
                 {
                     Id = p.Id,
                     FullName = p.FullName,
                     IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                     EncryptedMessages = p.Mails.Select(m => new EncryptedMessagesDto
                     {
                         Description = m.Description
                     }).ToArray()
                     
                 })
                 .ToArray();

            var sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportPrisonersDto[]), 
                new XmlRootAttribute("Prisoners"));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            serializer.Serialize(new StringWriter(sb), prisoners, ns);
            return sb.ToString();
        }
    }
}