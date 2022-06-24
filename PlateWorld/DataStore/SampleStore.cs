using PlateWorld.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.DataStore
{
    public class SampleStore
    {
        public event Action<ISample[]> SamplesAdded;
        public event Action<ISample[]> SamplesRemoved;
        Dictionary<Guid, ISample> sampleDict { get; set; } 
            = new Dictionary<Guid, ISample>();

        public void AddSamples(IEnumerable<ISample> samples)
        {
            var sampA = samples.ToArray();
            foreach (var sample in sampA)
            {
                sampleDict[sample.Id] = sample;
                SamplesAdded?.Invoke(sampA);
            }
        }

        public bool ContainsSample(Guid plateId)
        {
            return sampleDict.ContainsKey(plateId);
        }

        public bool ContainsSampleName(string plateName)
        {
            return sampleDict.Values.Any(p => p.Name == plateName);
        }
        public IEnumerable<ISample> AllSamples
        {
            get { return sampleDict.Values; }
        }

        public bool[] RemovePlates(ISample[] samples)
        {
            var res = samples.Select(
                sample => sampleDict.Remove(sample.Id));
            SamplesRemoved?.Invoke(samples);
            return res.ToArray();
        }

    }
}
