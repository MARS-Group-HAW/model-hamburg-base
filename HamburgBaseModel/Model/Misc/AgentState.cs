namespace HamburgBaseModel.Misc
{
    /**
     * The enum AgentState is used to track the agent's current state/activity during the simulation.
     * This is a simple way to define different behavioral subroutines for an agent based on what its current
     * circumstances are. In this case, the enum tracks the Patient's travel and from to a DoctorsOffice
     */
    public enum AgentState
    {
        Sick,
        InTreatment,
        Cured
    }
}