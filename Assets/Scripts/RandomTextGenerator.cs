using UnityEngine;

public static class RandomTextGenerator
{
    private static readonly string[] Adjectives =
    {
        "aged", "ancient", "autumn", "billowing", "bitter", "black", "blue", "bold",
        "broad", "broken", "calm", "cold", "cool", "crimson", "curly", "damp",
        "dark", "dawn", "delicate", "divine", "dry", "empty", "falling", "fancy",
        "flat", "floral", "fragrant", "frosty", "gentle", "green", "hidden", "holy",
        "icy", "jolly", "late", "lingering", "little", "lively", "long", "lucky",
        "misty", "morning", "muddy", "mute", "nameless", "noisy", "odd", "old",
        "orange", "patient", "plain", "polished", "proud", "purple", "quiet", "rapid",
        "raspy", "red", "restless", "rough", "round", "royal", "shiny", "shrill",
        "shy", "silent", "small", "snowy", "soft", "solitary", "sparkling", "spring",
        "square", "steep", "still", "summer", "super", "sweet", "throbbing", "tight",
        "tiny", "twilight", "wandering", "weathered", "white", "wild", "winter", "wispy",
        "withered", "yellow", "young"
    };

    private static readonly string[] Nouns =
    {
        "art", "band", "bar", "base", "bird", "block", "boat", "bonus",
        "bread", "breeze", "brook", "bush", "butterfly", "cake", "cell", "cherry",
        "cloud", "credit", "darkness", "dawn", "dew", "disk", "dream", "dust",
        "feather", "field", "fire", "firefly", "flower", "fog", "forest", "frog",
        "frost", "glade", "glitter", "grass", "hall", "hat", "haze", "heart",
        "hill", "king", "lab", "lake", "leaf", "limit", "math", "meadow",
        "mode", "moon", "morning", "mountain", "mouse", "mud", "night", "paper",
        "pine", "poetry", "pond", "queen", "rain", "recipe", "resonance", "rice",
        "river", "salad", "scene", "sea", "shadow", "shape", "silence", "sky",
        "smoke", "snow", "snowflake", "sound", "star", "sun", "sun", "sunset",
        "surf", "term", "thunder", "tooth", "tree", "truth", "union", "unit",
        "violet", "voice", "water", "water", "waterfall", "wave", "wildflower", "wind",
        "wood"
    };

    private static readonly string LoremIpsumTemplate =
        "IN PARVIS ENIM SAEPE, QUI NIHIL EORUM COGITANT, " +
        "SI QUANDO IIS LUDENTES MINAMUR PRAECIPITATUROS ALICUNDE, " +
        "EXTIMESCUNT. QUOD IDEM CUM VESTRI FACIANT, NON SATIS MAGNAM " +
        "TRIBUUNT INVENTORIBUS GRATIAM. IAM IN ALTERA PHILOSOPHIAE PARTE. " +
        "AMPULLA ENIM SIT NECNE SIT, QUIS NON IURE OPTIMO IRRIDEATUR, " +
        "SI LABORET? NIHIL AD REM! NE SIT SANE; QUOD AUTEM RATIONE ACTUM " +
        "EST, ID OFFICIUM APPELLAMUS. AT ISTE NON DOLENDI STATUS NON VOCATUR " +
        "VOLUPTAS. DOLERE MALUM EST: IN CRUCEM QUI AGITUR, BEATUS ESSE NON " +
        "POTEST. QUIS SUAE URBIS CONSERVATOREM CODRUM, QUIS ERECHTHEI FILIAS NON MAXIME LAUDAT?";

    public static string GetRandomName()
    {
        return $"{Adjectives[Random.Range(0, Adjectives.Length)]} {Nouns[Random.Range(0, Nouns.Length)]}"
            .ToUpperInvariant();
    }

    public static string GetRandomDescription()
    {
        const int minRange = 15;
        const int maxRange = 30;
        var range = Random.Range(minRange, maxRange);
        var startIndex = Random.Range(0, LoremIpsumTemplate.Length - range);
        return LoremIpsumTemplate.Substring(startIndex, range);
    }
}