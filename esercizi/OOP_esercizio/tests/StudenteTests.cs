using System.Linq;
using GestioneStudenti;
using Xunit;

namespace GestioneStudenti.Tests
{
    public class StudenteTests
    {
        [Fact(Skip = "Rimuovi questo Skip una volta completata la Milestone 1")]
        public void AggiungiVoto_DeveInserireIlVotoNellaLista()
        {
            var studente = new Studente("Mario", "Rossi");

            studente.AggiungiVoto(28);

            Assert.Contains(28, studente.Voti);
        }

        [Fact(Skip = "Rimuovi questo Skip una volta completata la Milestone 2")]
        public void CalcolaMedia_DeveRestituireLaMediaDeiVoti()
        {
            var studente = new Studente("Maria", "Verdi");
            studente.AggiungiVoto(30);
            studente.AggiungiVoto(24);
            studente.AggiungiVoto(26);

            var media = studente.CalcolaMedia();

            Assert.Equal(26.666, media, precision: 3);
        }

        [Fact(Skip = "Rimuovi questo Skip una volta completata la Milestone 4")]
        public void StudenteUniversitario_ApplicaBonusAllaMedia()
        {
            var studente = new StudenteUniversitario("Giulia", "Neri", "Ingegneria Informatica");
            studente.AggiungiVoto(28);
            studente.AggiungiVoto(30);
            studente.AggiungiVoto(27);
            studente.AggiungiVoto(25);
            studente.AggiungiVoto(29);
            studente.AggiungiVoto(30);

            var media = studente.CalcolaMedia();

            Assert.True(media > 28, "La media dovrebbe includere il bonus di +1");
        }
    }
}
