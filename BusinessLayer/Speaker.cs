using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? YearExperience { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<BusinessLayer.Session> Sessions { get; set; }

        void ValidateExceptions()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentNullException("First Name is required");
            if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentNullException("Last name is required.");
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentNullException("Email is required.");
        }

        bool IsGood()
        {
            const int MaxYearsExperience = 10;
            const int MaxCertifications = 3;

            List<string> employess = new List<string>() { "Microsoft", "Google", "Fog Creek Software", "37Signals" };

            return YearExperience > MaxYearsExperience
                          || HasBlog
                          || Certifications.Count() > MaxCertifications || employess.Contains(Employer);
        }

        bool IsNavigatorDomainValidate()
        {
            const int MaxVersionBrowser = 9;

            List<string> domains = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };

            string emailDomain = Email.Split('@').Last();

            return domains.Contains(emailDomain) || 
                       (Browser.Name == WebBrowser.BrowserName.InternetExplorer && 
                       Browser.MajorVersion < MaxVersionBrowser);
        }

        public int getRegistrationFee(int? Experience)
        {
            int RegistrationFee = 0;
            if (Experience <= 0)
            {
                RegistrationFee = 500;
            }
            if (Experience >= 2 && Experience <= 3)
            {
                RegistrationFee = 250;
            }
            else if (Experience >= 4 && Experience <= 5)
            {
                RegistrationFee = 100;
            }
            else if (Experience >= 6 && Experience <= 9)
            {
                RegistrationFee = 50;
            }
            else
            {
                RegistrationFee = 0;
            }
            return RegistrationFee;
        }


        void validateFeatures()
        {
            bool isGood = false;
            bool isNavigatorDomain = false;

            isGood = IsGood();
            isNavigatorDomain = IsNavigatorDomainValidate();

            var isAcepted = isGood || !isNavigatorDomain;
            if (!isAcepted)
            {
                throw new SpeakerDoesntMeetRequirementsException("Speaker doesn't meet our abitrary and capricious standards.");
            }
        }

        void validateSession()
        {
            if (Sessions.Count() == 0) throw new ArgumentException("Can't register speaker with no sessions to present.");

            foreach (var session in Sessions)
            {
                session.Approved = !getSessionByOldTechnologies(session);
            }

            if (Sessions.Where(x => x.Approved).ToList().Count == 0)
            {
                throw new NoSessionsApprovedException("No sessions approved.");
            }
        }

        public int? Register(IRepository repository)
        {

            int? speakerId = null;

            ValidateExceptions();

            validateFeatures();

            validateSession();

            RegistrationFee = repository.GetRegistrationFee(YearExperience);

            speakerId = repository.SaveSpeaker(this);

            return speakerId;
        }

        bool getSessionByOldTechnologies(Session session)
        {

            var oldTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            return oldTechnologies.Any(c => session.Title.Contains(c)
                                                            || session.Description.Contains(c));
          
        }

        #region Custom Exceptions
        public class SpeakerDoesntMeetRequirementsException : Exception
        {
            public SpeakerDoesntMeetRequirementsException(string message)
                : base(message)
            {
            }

            public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
                : base(string.Format(format, args)) { }
        }

        public class NoSessionsApprovedException : Exception
        {
            public NoSessionsApprovedException(string message)
                : base(message)
            {
            }
        }
        #endregion
    }
}