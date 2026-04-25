//здесь используется базовый функционал C# и больше ничего
namespace TowerDefense
{
    public enum Sound    // enum и его методы (см. ниже)
    {
        BGM,
        Arrow,
        Arrowhit,
    }
    public static class SoundExtensions  //нужен статичный класс, внутри которого есть статичный метод
        //Это есть "Метод расширения".  Здесь мы имеем метод расширения для enum Sound.
    {
        public static void Play(this Sound sound) //если здесь я перед первым параметром ставлю слово this,
            //то после этого метод можно вызывать с этого параметра. 
        {
            // Sound.BGM.Play();  //То есть беру одного из enum-ов и на нём что-то вызываю.
            SoundPlayer.Instance.Play(sound);
        }
    }
}

