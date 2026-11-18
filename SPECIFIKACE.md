# Specifikace

## Slovník pojmů
| Pojem | Význam |
|---|---|
| Pořadatel | Uživatel programu, řídí turnaj. |
| Hráč | Osoba s jménem a jedinečnou přezdívkou. Přezdívka je identifikátor (nezáleží na velikosti písmen). |
| Turnaj | Soutěž s názvem, disciplínou a počtem kol. Otevřený je vždy jen jeden. |
| Disciplína | Hra turnaje (např. šachy, piškvorky). Program ji jen eviduje jako text. |
| Přihlášený hráč | Hráč zařazený do otevřeného turnaje. |
| Kolo | Jedna etapa turnaje, má číslo, zápasy a případně volný los. Je otevřené nebo uzavřené. |
| Zápas | Dvojice dvou různých hráčů v jednom kole s výsledkem nebo bez. |
| Výsledek | Výhra 1. hráče, výhra 2. hráče, remíza, případně dosud nezadáno. |
| Odehraný zápas | Zápas, který má zadaný výsledek. |
| Volný los | Hráč, který v kole nemá soupeře. |
| Začátek turnaje | Okamžik vytvoření prvního kola. Od něj nejde měnit přihlášky. |
| Uzavřené kolo | Kolo, ve kterém mají výsledek všechny zápasy. Výsledky už nejdou měnit. |
| Tabulka | Seřazený přehled hráčů: pořadí, hráč, body, odehrané zápasy. |

## Pravidla bodování (návrh k potvrzení zákazníkem)
| Situace | Body |
|---|---|
| Výhra | 2 |
| Remíza | 1 každému |
| Prohra | 0 |
| Volný los | 2 (jako výhra) |

- Volný los se počítá do bodů, ale **nezapočítává** se do počtu odehraných zápasů.
- Zápas bez výsledku se do bodů ani do počtu odehraných zápasů nepočítá.
- Řazení: body sestupně. Při shodě bodů mají hráči stejné pořadí a jsou seřazeni abecedně podle přezdívky (rozstřel se zatím neřeší).

## Pravidla a omezení
1. Přezdívka je jedinečná a neprázdná, jméno je neprázdné.
2. Hráče nejde odebrat, pokud má odehraný zápas nebo je přihlášen do otevřeného turnaje.
3. Otevřený je nejvýše jeden turnaj. Počet kol je celé číslo ≥ 1.
4. Přihlašovat a odhlašovat jde jen před vytvořením prvního kola.
5. Nové kolo jde vytvořit, jen když předchozí je uzavřené a počet kol nepřekročí zadaný.
6. Hráč je v kole nejvýše jednou (v dvojici, nebo jako volný los). Hráč nehraje sám proti sobě.
7. Výsledek jde zadat jen existujícímu zápasu, přepsat ho jde do uzavření kola.
8. Kolo jde uzavřít, jen když mají výsledek všechny zápasy.
9. Každý vstup z klávesnice je validován, chyba nikdy nezpůsobí pád.

## Seznam funkcí
| Oblast | Funkce |
|---|---|
| Hráči | přidat, odebrat, vypsat |
| Turnaj | založit, přihlásit hráče, odhlásit hráče, zobrazit stav |
| Kola | vytvořit kolo, přidat dvojici, přidělit volný los, vypsat kolo |
| Zápasy | zadat výsledek, přepsat výsledek, uzavřít kolo |
| Tabulka | zobrazit tabulku (přepočet podle pravidel) |
| Soubor | uložit, načíst, ošetření chybějícího a poškozeného souboru |
| Ostatní | konzolové menu, ošetření vstupů |

## Příklad pro ověření tabulky
Hráči A, B, C, D, E; 3 kola. Postup a porovnání s programem doplní T22.
