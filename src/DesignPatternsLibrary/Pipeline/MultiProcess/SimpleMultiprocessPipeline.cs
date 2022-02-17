using DesignPatternsLibrary.ProcessRunner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiProcess
{
    internal class SimpleMultiprocessPipeline
    {
        public List<Job> JobTasks { get; } = new List<Job>(); 

        public void AddStep(string processName, string arguments)
        {
            JobTasks.Add(new Job(processName, arguments));
        }

        public async Task Execute() 
        {
            foreach (Job task in JobTasks)
            {
                task.Results = await AsyncProcessRunner.Run(task.ProcessName, task.Arguments);
            }
        }
    }

    class Job
    {
        public Job(string processName, string arguments)
        {
            ProcessName = processName;
            Arguments = arguments;
        }

        public string ProcessName { get; }
        public string Arguments { get; }
        public ProcessResults Results { get; set; }
    }
}
