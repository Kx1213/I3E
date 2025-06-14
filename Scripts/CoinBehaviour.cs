using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    MeshRenderer MyMeshRenderer;
    public int value = 1;

    [SerializeField]
    Material hightlightMat;
    Material originalMat;

    void Start() 
    {
        MyMeshRenderer = GetComponent<MeshRenderer>(); 
        originalMat = MyMeshRenderer.material;
    }

    public void Highlight() 
    {
        MyMeshRenderer.material = hightlightMat; 
    }

    public void UnHighlight() 
    {
        MyMeshRenderer.material = originalMat; 
    }

    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(value);
        Destroy(gameObject);
    }
}