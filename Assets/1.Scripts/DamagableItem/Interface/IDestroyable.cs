

public interface IDestroyable 
{
    /// <summary>
    /// 파괴 
    /// </summary>
    /// <param name = "speed">파괴 시 필요한 속도(힘, 파워)</param>
    /// <param name = "force">파괴 시 날라가는 힘</param>
    void Destory(float speed, float force);
}
