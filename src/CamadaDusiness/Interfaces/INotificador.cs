

using CamadaBusiness.Notifications;

namespace CamadaBusiness.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);    
    }
}
