using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;


namespace TowerDefense
{
    [CreateAssetMenu]
    public class TowerAsset: ScriptableObject
    {
        public int gold;
        public Sprite sprite;
        public Sprite GUISprite;
        public TurretProperties turretProperties;
        public float radius;
       
    }
}

//я расписал цепочку появления разных башен, вот она: 
//1.есть скрипт TowerAsset - это ScriptableObject
//2. Создаю нужное кол-во SO  типа TowerAsset 
//3. Вставляю в эти SO разные спрайты башен.
//4. есть скрипт TowerBuyControl, который висит на каждой башне в иерархии.
//Каждая башня - это есть префаб TowerBuyControl(объект-кнопка, по кот. жму, чтоб в сцене появилась
//башня). И в этом скрипте хранятся: ассет конкретной башни.
//5. Внутри префаба TowerBuyControl дочерним объектом помещена Button,
//где в OnClick указан метод Buy скрипта TowerBuyControl.
//6. Таким образом, когда пользователь жмёт на эту кнопку,
//вызывается метод Buy из скрипта TowerBuyControl, который вызывает
//метод TryBuild из скрипта TDPlayer с передачей в аргументе ассета башни.
//7. TryBuild  инстанциирует ДРУГОЙ ПРЕФАБ - префаб самой башни. Сам этот префаб хранится 
//в скрипте TDPlayer, а скрипт TDPlayer висит на отдельном объекте в иерархии PlayerCamp.
//8. Далее TryBuild  здесь же у инстанциированного префаба вынимает поле спрайт
//у SpriteRenderer-а и присваивает ему спрайт из ассета.