namespace ActivityMapper.Demo.Models
{
    using Newtonsoft.Json;
    using ActivityMapper.Interfaces;

    public class SampleActivityInput : IActivityInput
    {
        [JsonRequired]
        public string Name { get; set; }
    }
}
