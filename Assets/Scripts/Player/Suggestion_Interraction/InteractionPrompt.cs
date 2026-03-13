using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractionPrompt : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI promptText;
    public Camera playerCamera;

    [Header("Settings")]
    public float raycastDistance = 3f;
    public LayerMask interactableLayers = ~0;

    [Header("Tag Prompts")]
    public List<TagPrompt> tagPrompts = new List<TagPrompt>
    {
        new TagPrompt { tag = "Placeholder_1",  prompt = "Press E to mine"   },
    };

    // ── internal ──────────────────────────────────────────────────────────────

    private Dictionary<string, string> _promptLookup;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        BuildLookup();
        HidePrompt();
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, interactableLayers))
        {
            if (_promptLookup.TryGetValue(hit.collider.tag, out string message))
            {
                ShowPrompt(message);
                return;
            }
        }

        HidePrompt();
    }

    // ── public helpers ────────────────────────────────────────────────────────

    /// <summary>Add a new tag/prompt pair at runtime.</summary>
    public void RegisterPrompt(string tag, string prompt)
    {
        // Update or add in both the list (for inspector visibility) and the lookup.
        TagPrompt existing = tagPrompts.Find(t => t.tag == tag);
        if (existing != null)
            existing.prompt = prompt;
        else
            tagPrompts.Add(new TagPrompt { tag = tag, prompt = prompt });

        _promptLookup[tag] = prompt;
    }

    /// <summary>Remove a tag/prompt pair at runtime.</summary>
    public void UnregisterPrompt(string tag)
    {
        tagPrompts.RemoveAll(t => t.tag == tag);
        _promptLookup.Remove(tag);
    }

    // ── private helpers ───────────────────────────────────────────────────────

    private void BuildLookup()
    {
        _promptLookup = new Dictionary<string, string>();
        foreach (TagPrompt tp in tagPrompts)
        {
            if (!string.IsNullOrEmpty(tp.tag))
                _promptLookup[tp.tag] = tp.prompt;
        }
    }

    private void ShowPrompt(string message)
    {
        if (promptText == null) return;
        promptText.text    = message;
        promptText.enabled = true;
    }

    private void HidePrompt()
    {
        if (promptText == null) return;
        promptText.enabled = false;
    }
}

/// <summary>
/// One tag → prompt mapping, visible and editable in the Inspector.
/// Add as many entries as you need in the Tag Prompts list.
/// </summary>
[System.Serializable]
public class TagPrompt
{
    public string tag;
    [TextArea(1, 2)]
    public string prompt;
}
