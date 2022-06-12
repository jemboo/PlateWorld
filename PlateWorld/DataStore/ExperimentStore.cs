using System;
using System.Linq;
using System.Collections.Generic;
using PlateWorld.Models.BasicTypes;

namespace ExperimentWorld.DataStore
{
    public class ExperimentStore
    {
        public event Action<IExperiment[]> ExperimentsAdded;
        public event Action<IExperiment[]> ExperimentsRemoved;
        Dictionary<Guid, Experiment> experimentDict { get; set; } 
            = new Dictionary<Guid, Experiment>();

        public void AddExperiments(Experiment[] experiments)
        {
            foreach (var experiment in experiments)
            {
                experimentDict[experiment.Id] = experiment;
            }
            ExperimentsAdded?.Invoke(experiments);
        }

        public bool ContainsExperiment(Guid experimentId)
        {
            return experimentDict.ContainsKey(experimentId);
        }

        public bool ContainsExperimentName(string experimentName)
        {
            return experimentDict.Values.Any(p => p.Name == experimentName);
        }
        public IEnumerable<Experiment> AllExperiments 
        {
            get { return experimentDict.Values; }
        }

        public bool[] RemoveExperiments(IExperiment[] experiments)
        {
            var res = experiments.Select(
                experiment => experimentDict.Remove(experiment.Id));
            ExperimentsRemoved?.Invoke(experiments);
            return res.ToArray();
        }

    }
}
