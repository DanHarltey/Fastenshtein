namespace SpeedTestApp
{
    internal class LevenshteinBase
    {
        private readonly string value1;

        public LevenshteinBase(string value1)
        {
            this.value1 = value1;
        }

        protected string Value1
        {
            get
            {
                return this.value1;
            }
        }
    }
}
