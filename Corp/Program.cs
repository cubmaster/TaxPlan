using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RulesEngine.Models;
using static RulesEngine.Extensions.ListofRuleResultTreeExtension;
namespace Corp
{
    class Program
    {
        static void Main(string[] args)
        {
            var scenario = new Scenario();
            
            var rule = new RulesEngine.RulesEngine(scenario.Workflows.ToArray());
            var ruleParams = new List<RuleParameter>();
            ruleParams.Add(new RuleParameter("data",scenario.Data));
            var scenarioResults = new Dictionary<string, string>();

            foreach (var wf in scenario.Workflows)
            {
                Console.WriteLine(wf.WorkflowName);
                var output = new Dictionary<string, object>();
                foreach (var r in wf.Rules)
                {
                   
                    
                    Console.WriteLine(r.RuleName);
                    var result = rule.ExecuteActionWorkflowAsync(wf.WorkflowName, r.RuleName, ruleParams.ToArray());
                    result.Result.Results.OnSuccess(x => { Console.WriteLine($"{x}:{result.Result.Output}"); });
                    result.Result.Results.OnFail(() => { Console.WriteLine(r.ErrorMessage); });
                    output.Add(r.RuleName,result.Result.Output);
                    
                }
                ruleParams.Add(new RuleParameter(wf.WorkflowName,output));
            }
           
            Console.ReadLine();
        }
    }
}