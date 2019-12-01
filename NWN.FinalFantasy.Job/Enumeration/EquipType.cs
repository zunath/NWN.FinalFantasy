namespace NWN.FinalFantasy.Job.Enumeration
{
    public enum EquipType
    {
        /// <summary>
        /// This denotes that the ability is able to be equipped on other jobs.
        /// </summary>
        CrossJob,
        /// <summary>
        /// This denotes that the ability is only useable on the jobs which learn it.
        /// It is automatically granted.
        /// </summary>
        SingleJob,
        /// <summary>
        /// This denotes that the ability is cross-job but all jobs must
        /// manually equip it.
        /// </summary>
        CrossJobManuallyEquipped
    }
}
