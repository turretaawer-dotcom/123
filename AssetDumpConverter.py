import json

# File paths
asset_dump_file = 'data/AssetDump.txt'
custom_prefab_file = 'data/customprefab.JSON'
output_file = 'unique_objects.txt'

# Read AssetDump.txt and maintain order in a list of tuples (asset_name, asset_id)
asset_list = []
with open(asset_dump_file, 'r') as file:
    for line in file:
        # Extract asset path and ID from each line
        parts = line.split(':')
        if len(parts) >= 2:
            asset_name = parts[0].strip()
            asset_id_part = parts[1].strip()
            if asset_id_part.startswith('Hash='):
                try:
                    asset_id = int(asset_id_part.split('=')[1])
                    asset_list.append((asset_name, asset_id))
                except (IndexError, ValueError):
                    pass  # Skip lines where the asset_id is not a valid integer

# Read the customprefab.JSON file
with open(custom_prefab_file, 'r') as file:
    custom_prefab_data = json.load(file)

# Extract unique IDs from the customprefab JSON
unique_ids = set()
for prefab in custom_prefab_data.get('prefabs', []):
    unique_ids.add(prefab['id'])

# Filter the asset list to only include assets whose IDs are present in customprefab.JSON
unique_objects = [(asset_name, asset_id) for asset_name, asset_id in asset_list if asset_id in unique_ids]

# Write the unique objects to the output file, maintaining the original order
with open(output_file, 'w') as file:
    for asset_name, asset_id in unique_objects:
        file.write(f'{asset_id} : "{asset_name}",\n')

print(f'Successfully saved unique objects to {output_file}')
