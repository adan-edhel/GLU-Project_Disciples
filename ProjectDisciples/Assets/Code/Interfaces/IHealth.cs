using Photon.Pun;
public interface IHealth : IPunObservable
{
    float Health { get; set; }

    void ResetHealth();

    void DealDamage(float Damage, EGameElement Element, string NicknameDamegeDealer);

    PhotonView GetPhotonView { get; }
}
