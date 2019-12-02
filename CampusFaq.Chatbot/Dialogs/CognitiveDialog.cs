using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;

namespace CampusFaq.Chatbot.Dialogs
{
    [LuisModel("0f2e72b6-65dd-4217-9ee3-11a584e6be29", "7405ef826602457f8b015b619203e1ec", domain: "https://westus.api.cognitive.microsoft.com")]
    [Serializable]
    public class CognitiveDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Não entendi. pode repetir?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("atendimentoEstagio")]
        public async Task AtendimentoEstagio(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Os horários de atendimento do estágio são topper.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("atendimentoSecretaria")]
        public async Task AtendimentoSecretaria(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("A secretaria está aberta de segunda a sexta em três turnos. Das 9h às 12h, " +
                "15h30 às 17h30 e a noite das 18h30 às 20h30.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("gradeHoraria")]
        public async Task GradeHoraria(IDialogContext context, LuisResult result)
        {
            EntityRecommendation curso;
            if (result.TryFindEntity("Curso", out curso))
            {
                if (curso.Entity == "análise e desenvolvimento de sistemas")
                {

                }
                else if (curso.Entity == "gestão de empresas")
                {

                }
                else if (curso.Entity == "polímeros")
                {

                }
                else if (curso.Entity == "logística")
                {

                }
                else if (curso.Entity == "comércio exterior")
                {

                }
            }

            await context.PostAsync("a grade horária para o curso de adsfasdfasd.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("reservaBiblioteca")]
        public async Task ReservaBiblioteca(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("É possível reservar um livro no balcão da biblioteca, você será avisado por telefone quando " +
                "a obra estiver disponível e terá 24 horas pra ir lá e fazer o empréstimo, senão o próximo na lista de espera será " +
                "chamado.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("documentosEstagio")]
        public async Task DocumentosEstagio(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Selecione um documento para baixar:");
            context.Wait(MessageReceived);
        }

    }
}