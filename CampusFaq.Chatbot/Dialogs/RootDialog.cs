using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace CampusFaq.Chatbot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> argument)
        {
            PromptDialog.Choice(
                context,
                FirstLevelAsync,
                new string[] { "Secretaria", "Biblioteca", "Área do aluno", "Suporte DTI", "Institucional" },
                "Olá, sou o bot de atendimento ao aluno da Fatec Zona Leste. Qual a sua dúvida?",
                "Selecione uma das opções",
                promptStyle: PromptStyle.Auto);
        }

        private async Task FirstLevelAsync(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            if (message.ToLower() == "secretaria")
            {
                await context.PostAsync("**Horários de atendimento da secretaria**\n\n" +
                    "Manhã - 09h às 12h\n\n" +
                    "Tarde - 15h30 às 17h30\n\n" +
                    "Noite - 18h30 às 20h30\n\n\n\n" +
                    "As solicitações online de documentação devem ser solicitadas pelo e-mail **fateczlsolicitacaodedocumento@gmail.com**\n\n" +
                    "Os formulários deverão ser preenchidos, datados, assinados e escaneados.");

                PromptDialog.Choice(
                    context,
                    SecondLevelAsync,
                    new string[] { "Download de formulários", "Funcionários da secretaria", "Prazos para entrega de documentos", "Voltar ao menu inicial" },
                    "Sobre Secretaria, escolha uma das opções abaixo",
                    "Selecione uma das opções",
                    promptStyle: PromptStyle.Auto);
            }
            else if (message.ToLower() == "biblioteca")
            {
                PromptDialog.Choice(
                    context,
                    SecondLevelAsync,
                    new string[] { "Consultar TCC", "Consultar acervo", "Periódicos científicos", "Voltar ao menu inicial" },
                    "Sobre Biblioteca, escolha uma das opções abaixo",
                    "Selecione uma das opções",
                    promptStyle: PromptStyle.Auto);
            }
            else if (message.ToLower() == "área do aluno")
            {
                PromptDialog.Choice(
                    context,
                    SecondLevelAsync,
                    new string[] { "Calendário acadêmico", "Estágio", "Jornal INFOTEC", "SIGA", "Transferência", "Voltar ao menu inicial" },
                    "Sobre Área do aluno, escolha uma das opções abaixo",
                    "Selecione uma das opções",
                    promptStyle: PromptStyle.Auto);
            }
            else if (message.ToLower() == "suporte dti")
            {
                await context.PostAsync("**E-mails do DTI**\n\n" +
                    "f111ti@cps.sp.gov.br\n\n" +
                    "local.ue111@cps.sp.gov.br\n\n\n\n" +
                    "**Responsáveis pelo DTI**\n\n" +
                    "Edson Company Colalto Junior - Analista de Suporte e Gestão\n\n" +
                    "José Maria da Silva - Analista de Suporte e Gestão\n\n" +
                    "Alessandra Souza da Silva - Estagiária");

                PromptDialog.Choice(
                    context,
                    SecondLevelAsync,
                    new string[] { "Abrir um chamado", "Voltar ao menu inicial" },
                    "Sobre Suporte, escolha uma das opções abaixo",
                    "Selecione uma das opções",
                    promptStyle: PromptStyle.Auto);
            }
            else if (message.ToLower() == "institucional")
            {
                PromptDialog.Choice(
                    context,
                    SecondLevelAsync,
                    new string[] { "Sobre a Fatec Zona Leste", "Localização", "Congregação", "CPA", "Voltar ao menu inicial" },
                    "Sobre Institucional, escolha uma das opções abaixo",
                    "Selecione uma das opções",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                PromptDialog.Choice(
                    context,
                    FirstLevelAsync,
                    new string[] { "Secretaria", "Biblioteca", "Área do aluno", "Suporte DTI", "Institucional" },
                    "Não entendi a sua mensagem, tente novamente selecionando uma das opções abaixo",
                    "Selecione uma das opções1",
                    promptStyle: PromptStyle.Auto);
            }
        }

        private async Task SecondLevelAsync(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            if (message.ToLower() == "voltar ao menu inicial")
            {
                PromptDialog.Choice(
                context,
                FirstLevelAsync,
                new string[] { "Secretaria", "Biblioteca", "Área do aluno", "Suporte DTI", "Institucional" },
                "Escolha um tópico",
                "Selecione uma das opções",
                promptStyle: PromptStyle.Auto);
            }
            else
            {
                PromptDialog.Choice(
                    context,
                    FirstLevelAsync,
                    new string[] { "Secretaria", "Biblioteca", "Área do aluno", "Suporte DTI", "Institucional" },
                    "Ainda não estou preparado pra responder essa questão. Faça uma nova busca.",
                    "Selecione uma das opções",
                    promptStyle: PromptStyle.Auto);
            }
        }
    }
}