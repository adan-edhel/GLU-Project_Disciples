using Photon.Pun;
public interface IHealth : IPunObservable
{
    float Health { get; set; }

   
    void DealDamage(float Damage, EGameElement Element);
    
}
