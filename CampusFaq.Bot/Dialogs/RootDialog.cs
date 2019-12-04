using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using static CampusFaq.Bot.Class.cEnums;

namespace CampusFaq.Bot.Dialogs
{
    [LuisModel("0f2e72b6-65dd-4217-9ee3-11a584e6be29", "7405ef826602457f8b015b619203e1ec")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        #region Params
        private static Random rnd = new Random();

        private string GRADE_ADS = ConfigurationManager.AppSettings["grade_ads"];
        private string GRADE_COMEX = ConfigurationManager.AppSettings["grade_comex"];
        private string GRADE_GESTAO_RH = ConfigurationManager.AppSettings["grade_gestao_rh"];
        private string GRADE_GESTAO_EMPRESARIAL = ConfigurationManager.AppSettings["grade_gestao_emp"];
        private string GRADE_LOGISTICA = ConfigurationManager.AppSettings["grade_logistica"];
        private string GRADE_POLIMEROS = ConfigurationManager.AppSettings["grade_polimeros"];
        private string GRADE_PLASTICOS = ConfigurationManager.AppSettings["grade_plasticos"];

        private string REGIME_DISCIPLINAR = ConfigurationManager.AppSettings["regime_disciplinar"];
        private string REGULAMENTO_GRADUACAO = ConfigurationManager.AppSettings["regulamento_graduacao"];
        private string REGIMENTO_UNIFICADO = ConfigurationManager.AppSettings["regimento_unificado"];
        private string SOLICITACAO_DOCUMENTO = ConfigurationManager.AppSettings["solicitacao_documento"];
        private string REQUERIMENTO_SECRETARIA = ConfigurationManager.AppSettings["requerimento_secretaria"];
        private string REQUERIMENTO_GLOBAL = ConfigurationManager.AppSettings["requerimento_global"];
        private string REQUERIMENTO_DATA_PROVA = ConfigurationManager.AppSettings["requerimento_data_prova"];
        private string REQUERIMENTO_DIRETOR = ConfigurationManager.AppSettings["requerimento_diretor"];
        private string REQUERIMENTO_COORDENADOR = ConfigurationManager.AppSettings["requerimento_coordenador"];
        private string FORMULARIO_BILHETE = ConfigurationManager.AppSettings["formulario_bilhete"];
        private string FORMULARIO_BOM = ConfigurationManager.AppSettings["formulario_bom"];

        private string GRADE_ESTAGIO = ConfigurationManager.AppSettings["grade_estagio"];
        #endregion

        #region Intents
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var arr = new List<string>()
            {
                "Do que vc tá falando, bicho. Acho que eu não sei te responder essa.",
                "Não entendi, pode repetir?",
                "Tem certeza que eu deveria entender isso aí? Tenta repetir a pergunta pra mim ou perguntar outra coisa pf",
                "Não compreendi vosso questionamento, pergunte-me acerca de tópicos de meu entendimento.",
                "Sei lá, acho que eu não sei isso aí não. Me pergunta outra coisa."
            };
            int r = rnd.Next(arr.Count);

            await context.PostAsync(arr[r]);
            context.Wait(MessageReceived);
        }

        [LuisIntent("saudacoes")]
        public async Task Saudacoes(IDialogContext context, LuisResult result)
        {
            var arr = new List<string>()
            {
                "Eai men, tudo bem?",
                "Fala, meu consagrado. De boa?",
                "Eai, meu camisa nove",
                "Alô, alô",
                "Positivo, meu operante",
                "Oi, como vai vc?"
            };
            int r = rnd.Next(arr.Count);

            await context.PostAsync(arr[r]);
            context.Wait(MessageReceived);
        }

        [LuisIntent("tudoBem")]
        public async Task TudoBem(IDialogContext context, LuisResult result)
        {
            var arr = new List<string>()
            {
                "Estou maravilhosamente bem tb, no que eu posso lhe ajudar hj?",
                "Tô com sono mas tudo bem, do que vc precisa hj?",
                "Estou de boa na lagoa",
                "Suave na nave, chora",
                "Entediado, precisa de algo?"
            };
            int r = rnd.Next(arr.Count);

            await context.PostAsync(arr[r]);
            context.Wait(MessageReceived);
        }

        [LuisIntent("sobre")]
        public async Task Sobre(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Eu sou um bot de atendimento que simula um primeiro atendimento a secretaria da FATEC. " +
                "A ideia é que vc possa buscar comigo assuntos básicos como grade horária de cursos, modelos de documentos " +
                "e outras informações que vc pode precisar mas que não valem uma ida na secretaria ou no próprio site da fatec." +
                ". Experimenta perguntar pra mim algo sobre horários de atendimento de estágio ou secretaria, reserva na biblioteca" +
                ", etc.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("atendimentoEstagio")]
        public async Task AtendimentoEstagio(IDialogContext context, LuisResult result)
        {
            Attachment img = new Attachment();
            img.ContentType = "image/png";
            img.ContentUrl = GRADE_ESTAGIO;

            var message = context.MakeMessage();
            message.Text = "Esta é a grade de atendimento dos professores de estágio nesse semestre.";
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(img);

            await context.PostAsync(message);
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
                var arr = curso.Resolution["values"];
                var text = ((List<object>)arr)[0].ToString();

                if (text == "análise e desenvolvimento de sistemas")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.ADS));
                else if (text == "gestão de empresas")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.GESTAO_EMPRESARIAL));
                else if (text == "gestão de rh")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.GESTAO_RH));
                else if (text == "polímeros")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.POLIMEROS));
                else if (text == "plasticos")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.PLASTICOS));
                else if (text == "logística")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.LOGISTICA));
                else if (text == "comércio exterior")
                    await context.PostAsync(criarGradeReply(context.MakeMessage(), Curso.COMEX));
                else
                    await context.PostAsync("Vc pode especificar o curso pra eu te mandar a grade horária? " +
                        "Ex: 'Quero a grade de ADS'");
            }

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
            EntityRecommendation documento;
            if (result.TryFindEntity("Documento", out documento))
            {
                var arr = documento.Resolution["values"];
                var text = ((List<object>)arr)[0].ToString();

                if (text == "regime disciplinar")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REGIME_DISCIPLINAR));
                else if (text == "regulamento de graduação")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REGULAMENTO_GRADUACAO));
                else if (text == "regimento unificado")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REGIMENTO_UNIFICADO));
                else if (text == "requerimento para solicitação de documento")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.SOLICITACAO_DOCUMENTO));
                else if (text == "requerimento para a secretaria")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REQUERIMENTO_SECRETARIA));
                else if (text == "requerimento global")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REQUERIMENTO_GLOBAL));
                else if (text == "requerimento data de prova")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REQUERIMENTO_DATA_PROVA));
                else if (text == "requerimento ao diretor")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REQUERIMENTO_DIRETOR));
                else if (text == "requerimento ao coordenador")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.REQUERIMENTO_COORDENADOR));
                else if (text == "Formulário do bilhete único")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.FORMULARIO_BILHETE));
                else if (text == "Formulário do bilhete BOM")
                    await context.PostAsync(criarDownloadDoc(context.MakeMessage(), Documento.FORMULARIO_BOM));
            }
            else
            {
                await context.PostAsync("Não entendi, tenta especificar pra mim qual documento vc precisa. " +
                    "Ex: 'quero o regulamento de graduação'");
            }

            context.Wait(MessageReceived);
        }
        #endregion

        #region Util methods
        public override Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);

            return Task.CompletedTask;
        }

        private IMessageActivity criarDownloadDoc(IMessageActivity message, Documento doc)
        {
            string url = "";
            switch (doc)
            {
                case Documento.REGIME_DISCIPLINAR:
                    url = REGIME_DISCIPLINAR;
                    break;
                case Documento.REGULAMENTO_GRADUACAO:
                    url = REGULAMENTO_GRADUACAO;
                    break;
                case Documento.REGIMENTO_UNIFICADO:
                    url = REGIMENTO_UNIFICADO;
                    break;
                case Documento.SOLICITACAO_DOCUMENTO:
                    url = SOLICITACAO_DOCUMENTO;
                    break;
                case Documento.REQUERIMENTO_SECRETARIA:
                    url = REQUERIMENTO_SECRETARIA;
                    break;
                case Documento.REQUERIMENTO_GLOBAL:
                    url = REQUERIMENTO_GLOBAL;
                    break;
                case Documento.REQUERIMENTO_DATA_PROVA:
                    url = REQUERIMENTO_DATA_PROVA;
                    break;
                case Documento.REQUERIMENTO_DIRETOR:
                    url = REQUERIMENTO_DIRETOR;
                    break;
                case Documento.REQUERIMENTO_COORDENADOR:
                    url = REQUERIMENTO_COORDENADOR;
                    break;
                case Documento.FORMULARIO_BILHETE:
                    url = FORMULARIO_BILHETE;
                    break;
                case Documento.FORMULARIO_BOM:
                    url = FORMULARIO_BOM;
                    break;
                default:
                    break;
            }


            HeroCard cardDownload = new HeroCard();
            cardDownload.Text = "Clique no botão abaixo pra baixar o documento solicitado.";
            cardDownload.Buttons = new List<CardAction>()
            {
                new CardAction()
                {
                    Type = ActionTypes.DownloadFile,
                    Value = url,
                    Text = "Download",
                    DisplayText = "Download",
                    Title = "Documento"
                }
            };

            message.Attachments = new List<Attachment>();
            message.Attachments.Add(cardDownload.ToAttachment());
            return message;
        }

        private IMessageActivity criarGradeReply(IMessageActivity message, Curso curso)
        {
            Attachment img = new Attachment();
            img.ContentType = "image/png";

            switch (curso)
            {
                case Curso.ADS:
                    img.ContentUrl = GRADE_ADS;
                    message.Text = $"Aqui está a grade horária do curso de ADS.";
                    break;
                case Curso.COMEX:
                    message.Text = $"Aqui está a grade horária do curso de COMEX.";
                    img.ContentUrl = GRADE_COMEX;
                    break;
                case Curso.LOGISTICA:
                    message.Text = $"Aqui está a grade horária do curso de logística.";
                    img.ContentUrl = GRADE_LOGISTICA;
                    break;
                case Curso.GESTAO_EMPRESARIAL:
                    message.Text = $"Aqui está a grade horária do curso de gestão empresarial.";
                    img.ContentUrl = GRADE_GESTAO_EMPRESARIAL;
                    break;
                case Curso.POLIMEROS:
                    message.Text = $"Aqui está a grade horária do curso de polímeros.";
                    img.ContentUrl = GRADE_POLIMEROS;
                    break;
                case Curso.PLASTICOS:
                    message.Text = $"Aqui está a grade horária do curso de produção de plásticos.";
                    img.ContentUrl = GRADE_PLASTICOS;
                    break;
                case Curso.GESTAO_RH:
                    message.Text = $"Aqui está a grade horária do curso de gestão de RH.";
                    img.ContentUrl = GRADE_GESTAO_RH;
                    break;
                default:
                    break;
            }

            message.Attachments = new List<Attachment>();
            message.Attachments.Add(img);

            return message;
        }
        #endregion
    }
}