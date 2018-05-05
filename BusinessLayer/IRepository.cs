namespace BusinessLayer
{
    public interface IRepository
	{
		int SaveSpeaker(Speaker speaker);

        int GetRegistrationFee(int? yearsExperience);
    }
}
