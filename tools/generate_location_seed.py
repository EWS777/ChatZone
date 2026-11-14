import collections
import os
import re
import sys
import unicodedata

MAX_CITIES = None
MAX_CITIES_PER_COUNTRY = None
SKIP_COUNTRIES = {"AN", "CS"}
SKIP_FEATURE_CODES = {"PPLX", "PPLH", "PPLW", "PPLQ"}
SKIP_LANGUAGES = {"bh", "sh"}
LANGUAGE_NAME_OVERRIDES = {"el": "Greek"}

src = sys.argv[1]
out = os.path.join(os.path.dirname(__file__), "..", "ChatZone.Api", "Shared", "Context", "Seed", "Data")
os.makedirs(out, exist_ok=True)


def clean(value):
    return value.replace("\t", " ").strip()


def name_key(country, name):
    name = "".join(ch for ch in unicodedata.normalize("NFKD", name) if not unicodedata.combining(ch))
    return country, name.casefold()


countries = []
for line in open(os.path.join(src, "countryInfo.txt"), encoding="utf-8"):
    if line.startswith("#"):
        continue
    f = line.rstrip("\n").split("\t")
    if f[0] not in SKIP_COUNTRIES:
        countries.append((f[0], clean(f[4])))
country_codes = {c[0] for c in countries}

cities = []
for line in open(os.path.join(src, "cities15000.txt"), encoding="utf-8"):
    f = line.rstrip("\n").split("\t")
    if f[7] in SKIP_FEATURE_CODES or f[8] not in country_codes:
        continue
    cities.append((int(f[14]), f[8], clean(f[1]), int(f[0])))
cities.sort(key=lambda c: (-c[0], c[3]))

selected, per_country, used_names = [], collections.Counter(), set()
for population, country, name, geo_id in cities:
    if name_key(country, name) in used_names:
        continue
    used_names.add(name_key(country, name))
    if MAX_CITIES_PER_COUNTRY is not None and per_country[country] >= MAX_CITIES_PER_COUNTRY:
        continue
    per_country[country] += 1
    selected.append((geo_id, country, name, population))
    if MAX_CITIES is not None and len(selected) == MAX_CITIES:
        break

languages = []
lines = open(os.path.join(src, "iso-languagecodes.txt"), encoding="utf-8").read().splitlines()[1:]
for line in lines:
    f = line.split("\t")
    code = f[2].strip()
    if not code or code in SKIP_LANGUAGES:
        continue
    name = LANGUAGE_NAME_OVERRIDES.get(code, re.sub(r"\s*\(.*\)$", "", f[3]))
    languages.append((code, clean(name)))


def write(file_name, header, rows, sort_key):
    with open(os.path.join(out, file_name), "w", encoding="utf-8", newline="\n") as fh:
        fh.write("\t".join(header) + "\n")
        for row in sorted(rows, key=sort_key):
            fh.write("\t".join(str(v) for v in row) + "\n")


write("countries.tsv", ["IsoCode", "Name"], countries, lambda r: r[1])
write("cities.tsv", ["GeoNameId", "CountryIsoCode", "Name", "Population"], selected, lambda r: (r[1], -r[3], r[0]))
write("languages.tsv", ["IsoCode", "Name"], languages, lambda r: r[1])
print(f"countries={len(countries)} cities={len(selected)} languages={len(languages)}")