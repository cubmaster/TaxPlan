using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Models;

namespace Corp
{
    public class Scenario
    {
        public Scenario()
        {
            var workflowFiles = Directory.GetFiles($"{Directory.GetCurrentDirectory()}/Scenarios", "Current.json", SearchOption.AllDirectories);
            var workflowData = File.ReadAllText(workflowFiles[0]);
            Workflows = JsonConvert.DeserializeObject<List<Workflow>>(workflowData);
            var transcriptFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "Transcript.json", SearchOption.AllDirectories);
            var transcriptData = File.ReadAllText(transcriptFiles[0]); 
            var converter = new ExpandoObjectConverter();
            Data = JsonConvert.DeserializeObject<ExpandoObject>(transcriptData, converter);
    
          
        }

        public  List<Workflow> Workflows { get; set; }
        public dynamic Data { get; set; }
    } 
    
}