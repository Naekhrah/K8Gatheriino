namespace K8GatherBotv2.Locales
{
    using System.Collections.Generic;

    /// <summary>
    /// The finnish localization.
    /// </summary>
    /// <seealso cref="ILocalization" />
    public class Finnish : ILocalization
    {
        private readonly Dictionary<Keys, string> localizations;

        /// <summary>
        /// Initializes a new instance of the <see cref="Finnish"/> class.
        /// </summary>
        public Finnish()
        {
            this.localizations = new Dictionary<Keys, string>
                {
                    { Keys.Ok, "Ok." },
                    { Keys.PickPhaseAlreadyInProcess, "Odota kunnes edellinen jono on käsitelty." },
                    { Keys.QueuePhaseAdded, "Lisätty!" },
                    { Keys.ReadyPhaseStarted, "Jono on nyt täynnä, merkitse itsesi valmiiksi käyttäen ***!ready*** komentoa. \n Aikaa 60 sekuntia!" },
                    { Keys.QueuePhaseAlreadyInQueue, "Olet jo jonossa!" },
                    { Keys.PickPhaseCannotRemove, "Liian myöhäistä peruuttaa, odota jonon käsittelyn valmistumista." },
                    { Keys.QueuePhaseRemoved, "Poistettu!" },
                    { Keys.QueuePhaseNotInQueue, "Et ole juuri nyt jonossa" },
                    { Keys.QueuePhaseNotReadyYet, "Jono ei ole vielä valmis!" },
                    { Keys.ReadyPhaseReady, "Valmiina!" },
                    { Keys.PickPhaseStarted, "Readycheck valmis, aloitetaan poimintavaihe! Ensimmäinen poiminta: Team 1" },
                    { Keys.PickPhaseTeamXCaptain, "{0}:n kapteeni: {1}" },
                    { Keys.PickPhaseInstructions, "Poimi pelaajia käyttäen ***!pick NUMERO***" },
                    { Keys.ReadyPhaseNotInQueue, "Näyttäisi siltä ettet ole jonossa." },
                    { Keys.PickPhasePlayedAdded, "Pelaaja lisätty {0}:n! \n {1}:n vuoro poimia!" },
                    { Keys.PickPhaseUnpicked, "***Poimimatta:***" },
                    { Keys.PickPhaseAlreadyPicked, "Pelaaja on jo joukkueessa!" },
                    { Keys.PickPhaseUnknownIndex, "Numerolla ei löytynyt pelaajaa!" },
                    { Keys.PickPhaseEveryoneNotReady, "Kaikki pelaavat eivät ole vielä valmiin!" },
                    { Keys.PickPhaseNotYourTurn, "Ei ole vuorosi poimia!" },
                    { Keys.PickPhaseNotCaptain, "Vain kapteenit voivat poimia pelaajia!" },
                    { Keys.QueuePhaseEmptyQueue, "Jono on tyhjä! Käytä ***!add*** aloittaaksesi jonon!" },
                    { Keys.AdminResetSuccessful, "Kaikki listat tyhjennetty onnistuneesti!" },
                    { Keys.StatusPickedTeams, "Valitut joukkueet" },
                    { Keys.StatusQueueStatus, "jonon tilanne" },
                    { Keys.InfoPurposeAnswer, "Saada pelaajia keräytymään pelien äärelle!" },
                    { Keys.InfoFunFactAnswer, "Gathut aiheuttavat paljon meemejä :thinking:" },
                    { Keys.InfoDeveloper, "Kehittäjä" },
                    { Keys.InfoPurpose, "Tarkoitus" },
                    { Keys.InfoFunFact, "Tiesitkö" },
                    { Keys.InfoCommands, "Komennot" },
                    { Keys.StatusQueuePlayers, "Pelaajat" },
                    { Keys.StatusNotReady, "EI VALMIINA" },
                    { Keys.ReadyPhaseTimeout, "Kaikki pelaajat eivät olleet valmiita readycheckin aikana. Palataan jonoon valmiina olleiden pelaajien kanssa." },
                    { Keys.ReadyPhaseAlreadyMarkedReady, "Olet jo merkinnyt itsesi valmiiksi!" },
                    { Keys.ReadyPhaseCannotAdd, "Odota poimintavaiheen päättymistä!" },
                    { Keys.FatKidHeader, "Viimeiseksi valittu" },
                    { Keys.FatKidTop10, "Top10 viimeisenä valitut" },
                    { Keys.FatKidStatusSingle, "{0} on valittu viimeisenä {1} kertaa ({2}/{3})" },
                    { Keys.HighScoresHeader, "Gathuja pelattu" },
                    { Keys.HighScoresTop10, "Top10 gathuLEGENDAT" },
                    { Keys.HighScoresStatusSingle, "{0} on pelannut {1} gathua ({2}/{3})" },
                    { Keys.ThinKidHeader, "1. varaus" },
                    { Keys.ThinKidTop10, "Top10 ensimmäisenä valitut" },
                    { Keys.ThinKidStatusSingle, "{0} on valittu ensimmäisenä {1} kertaa ({2}/{3})" },
                    { Keys.CaptainHeader, "Kapteeni" },
                    { Keys.CaptainTop10, "Top10 kapteenit" },
                    { Keys.CaptainStatusSingle, "{0} on valittu kapteeniksi {1} kertaa ({2}/{3})" },
                    { Keys.PlayerStats, "pelaajan tiedot" },
                    { Keys.RelinqPickPhaseStarted, "Olet jo valinnut pelaajan, liian myöhäistä luopua tehtävästä." },
                    { Keys.RelinqSuccessful, "Luovuit kapteeninhommista onnistuneesti, uusi kapteeni on: " }
                };
        }

        /// <inheritdoc />
        public string this[Keys key] => this.localizations.TryGetValue(key, out var text) ? text : key.ToString();
    }
}
