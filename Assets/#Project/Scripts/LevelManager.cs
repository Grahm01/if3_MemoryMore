using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public int row = 3;
    public int col = 3;

    public float gapRow = 1.5f;
    public float gapCol = 1.5f; //f sert à déclarer un float

    [Range(0f, 5f)] //permet de faire apparaite un slider entre 0 et 5
    public float timeBeforeReset = 1f;
    private bool resetOnGoing = false;
    public GameObject itemPrefab;

    public Material[] materials;
    public Material defaultMaterial;

    public ItemBehavior[] items;
    public List<int> selected = new List<int>();
    public List<int> matches = new List<int>();

    private Dictionary<int, Material> itemMaterial = new Dictionary<int, Material>();

    public UnityEvent whenPlayerWins;
    public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        items = new ItemBehavior[row * col];
        int index = 0;

        for(int x = 0; x < col; x++){
            for(int z = 0; z <row; z++){
                Vector3 position = new Vector3(x * gapCol, 0, z * gapRow);
                GameObject item = Instantiate(itemPrefab, position, Quaternion.identity);
                item.GetComponent<Renderer>().material =  defaultMaterial; // donne le materiaux de base
                
                items[index] = item.GetComponent<ItemBehavior>();

                items[index].id = index;
                items[index].manager = this;

                index++;
            }
        }
        
        GiveMaterial();

    }

    private void GiveMaterial(){
        List<int> possibilitis = new List<int>();
        for(int i = 0; i < row * col; i++){  // faut faire le row * col car il y a 12 possibilitis
            possibilitis.Add(i);
        }
        
        for(int i = 0; i <materials.Length; i++){ //c'est toujours mieux d'utiliser un for plutôt qu'un foreach, foreach c'est pas optimisé
            if(possibilitis.Count < 2) break;
            int idPoss = Random.Range(0, possibilitis.Count); //Lenght c'est pour un tableau et Count c'est pour une List
            int id1 = possibilitis[idPoss];
            possibilitis.RemoveAt(idPoss);
            idPoss = Random.Range(0, possibilitis.Count); 
            int id2 = possibilitis[idPoss];
            possibilitis.RemoveAt(idPoss);

            itemMaterial.Add(id1, materials[i]);
            itemMaterial.Add(id2, materials[i]);
        }
    }

    private IEnumerator ResetMaterials(int id1, int id2){
        resetOnGoing = true;
        //yield retunr 1 => retourn mais ne s'arrête pas donc on peut faire plusieurs yield d'affilé, ça s'appelle une coroutine
        yield return new WaitForSeconds(timeBeforeReset);
        ResetMaterial(id1);
        ResetMaterial(id2);
        resetOnGoing = false;
    }
    private IEnumerator Win(){
        yield return new WaitForSeconds(timeBeforeReset);
        whenPlayerWins?.Invoke();
    }
    public void RevealMaterial(int id){ //fonction pour révéler le materiel
        if(resetOnGoing == false && !selected.Contains(id) && !matches.Contains(id)){ //si l'id n'est pas dans la liste slected alors on l'ajoute et on montre son material
            selected.Add(id);
            Material material = itemMaterial[id];
            items[id].GetComponent<Renderer>().material = material;
            items[id].HasBeenSelected(true);
        }
    }
    // Update is called once per frame


    private void ResetMaterial(int id){
        //remettre le default material sur l'objet qui a pour id ce qui a été passé en paramèrtre
        items[id].GetComponent<Renderer>().material = defaultMaterial;
        items[id].HasBeenSelected(false);
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);


        if (selected.Count == 2) {
            if(itemMaterial[selected[0]]== itemMaterial[selected[1]]){
                matches.Add(selected[0]);
                matches.Add(selected[1]);
                items[selected[0]].HasBeenMatched();
                items[selected[1]].HasBeenMatched();

                if(matches.Count >= row * col){
                    PlayerPrefs.SetFloat("timer", timer);
                    StartCoroutine(Win());

                }
            }
            else{
                // reset
                StartCoroutine(ResetMaterials(selected[0], selected[1]));
                items[selected[0]].HasBeenSelected(false);
                items[selected[1]].HasBeenSelected(false);
            }
            selected.Clear();

        }


    }
}
