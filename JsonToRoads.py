import sys
import json
import math
import pyperclip
from math import degrees
import logging

# Set up logging
logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(levelname)s - %(message)s')

try:
    mapSize = int(input("input map size ex. 4000 or 4400:    "))
except ValueError:
    input("Invalid input. Please enter a valid integer.")
    sys.exit(1)

print("\nPlease drag and drop your JSON file into this window and press Enter:")
file_path = input().strip().strip('"')  # Remove any quotes that might be added by drag and drop

# Text snippet that goes at the beginning
beginText = """Begin Map
   Begin Level"""
    # Text snippet that goes at the end
endText = """
   End Level
Begin Surface
End Surface
End Map"""

def generate_prefix_text(Location):
    # Extract x, y, z from location tuple
    x = Location['x']
    y = Location['y'] 
    z = Location['z']
    text = f'''
      Begin Actor Class=/Game/Misc/BP_Splined_Mesh.BP_Splined_Mesh_C Name=BP_Splined_Mesh_C_4 Archetype="/Game/Misc/BP_Splined_Mesh.BP_Splined_Mesh_C'/Game/Misc/BP_Splined_Mesh.Default__BP_Splined_Mesh_C'" ExportPath="/Game/Misc/BP_Splined_Mesh.BP_Splined_Mesh_C'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4'"
         Begin Object Class=/Script/Engine.SceneComponent Name="DefaultSceneRoot" Archetype="/Script/Engine.SceneComponent'/Game/Misc/BP_Splined_Mesh.BP_Splined_Mesh_C:DefaultSceneRoot_GEN_VARIABLE'" ExportPath="/Script/Engine.SceneComponent'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.DefaultSceneRoot'"
         End Object
         Begin Object Class=/Script/Engine.SplineComponent Name="Spline" Archetype="/Script/Engine.SplineComponent'/Game/Misc/BP_Splined_Mesh.BP_Splined_Mesh_C:Spline_GEN_VARIABLE'" ExportPath="/Script/Engine.SplineComponent'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.Spline'"
         End Object
         Begin Object Class=/Script/Engine.SplineMeshComponent Name="NODE_AddSplineMeshComponent-1" Archetype="/Script/Engine.SplineMeshComponent'/Game/Misc/BP_Splined_Mesh.BP_Splined_Mesh_C:NODE_AddSplineMeshComponent-1'" ExportPath="/Script/Engine.SplineMeshComponent'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.NODE_AddSplineMeshComponent-1'"
            Begin Object Class=/Script/Engine.BodySetup Name="BodySetup_0" ExportPath="/Script/Engine.BodySetup'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.NODE_AddSplineMeshComponent-1.BodySetup_0'"
            End Object
         End Object
         Begin Object Name="DefaultSceneRoot" ExportPath="/Script/Engine.SceneComponent'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.DefaultSceneRoot'"
            RelativeLocation=(X={x},Y={y},Z={z})
            UCSSerializationIndex=0
            bNetAddressable=True
            CreationMethod=SimpleConstructionScript
         End Object'''
    return text

def generate_suffix_text(model):
    text=f'''
         Begin Object Name="NODE_AddSplineMeshComponent-1" ExportPath="/Script/Engine.SplineMeshComponent'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.NODE_AddSplineMeshComponent-1'"
            Begin Object Name="BodySetup_0" ExportPath="/Script/Engine.BodySetup'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_4.NODE_AddSplineMeshComponent-1.BodySetup_0'"
               DefaultInstance=(ObjectType=ECC_WorldStatic,CollisionProfileName="BlockAll")
            End Object
            SplineParams=(StartPos=(X=0.001000,Y=0.000000,Z=-0.000000),StartTangent=(X=1800.000061,Y=0.000000,Z=-0.000000),StartRoll=0.000000,EndPos=(X=1725.776633,Y=0.000000,Z=-0.000000),EndTangent=(X=1800.000061,Y=0.000000,Z=-0.000000),EndRoll=0.000000)
            SplineUpDir=(X=0.000000,Y=0.000000,Z=1.000000)
            CachedMeshBodySetupGuid=D9D2F0EF4B8108ED76FB1AB7A4A340F2
            BodySetup="/Script/Engine.BodySetup'BodySetup_0'"
            StaticMesh="/Script/Engine.StaticMesh'{model}'"
            StaticMeshImportVersion=1
            BodyInstance=(CollisionEnabled=QueryAndPhysics,CollisionProfileName="Custom")
            AttachParent="DefaultSceneRoot"
            UCSSerializationIndex=0
            bNetAddressable=True
            CreationMethod=UserConstructionScript
         End Object
         Spline="Spline"
         DefaultSceneRoot="DefaultSceneRoot"
         Splined Meshes(0)="/Script/Engine.StaticMesh'{model}'"
         Curve Scale=1.000000
         Use Instanced SM=True
         Mesh IDs Along Spline(1)=0
         Spacing=1800.000061
         Stretch Size=0.958765
         Number of Instances=1
         Ideal Stretch Size=0.958765
         Start Dist=0.001000
         End Dist=1725.776743
         Spline Mesh Component="NODE_AddSplineMeshComponent-1"
         RootComponent="DefaultSceneRoot"
         ActorLabel="BP_Splined_Mesh"
      End Actor'''
    return text

def generate_spline_text(points):
    """
    Generate Unreal Engine spline text from a list of (x,y,z) points
    points: list of tuples containing (x,y,z) coordinates
    """
    
    # Template for the beginning and end of the spline text
    header = '''
         Begin Object Name="Spline" ExportPath="/Script/Engine.SplineComponent\'/Game/Misc/Init.Init:PersistentLevel.BP_Splined_Mesh_C_15.Spline\'"'''
    footer = '''            bSplineHasBeenEdited=True
            bInputSplinePointsToConstructionScript=True
            AttachParent="DefaultSceneRoot"
            UCSSerializationIndex=0
            bNetAddressable=True
            CreationMethod=SimpleConstructionScript
         End Object'''

    # Generate position points
    position_points = []
    for i, point in enumerate(points):
        x, y, z = point
        
        # For simplicity, using the same value for arrive and leave tangents
        # In a real application, you might want to calculate these properly
        tangent_x = x * 0.1
        tangent_y = y * 0.1
        tangent_z = 0
        
        if i == 0:
            point_text = f'(ArriveTangent=(X={tangent_x},Y={tangent_y},Z={tangent_z}),LeaveTangent=(X={tangent_x},Y={tangent_y},Z={tangent_z}),InterpMode=CIM_CurveAuto)'
        else:
            point_text = f'(InVal={float(i)},OutVal=(X={x},Y={y},Z={z}),ArriveTangent=(X={tangent_x},Y={tangent_y},Z={tangent_z}),LeaveTangent=(X={tangent_x},Y={tangent_y},Z={tangent_z}),InterpMode=CIM_CurveAuto)'
        position_points.append(point_text)

    # Generate rotation points (using default values)
    rotation_points = []
    for i in range(len(points)):
        if i == 0:
            point_text = '(OutVal=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),ArriveTangent=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),LeaveTangent=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),InterpMode=CIM_CurveAuto)'
        else:
            point_text = f'(InVal={float(i)},OutVal=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),ArriveTangent=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),LeaveTangent=(X=0.000000,Y=0.000000,Z=0.000000,W=1.000000),InterpMode=CIM_CurveAuto)'
        rotation_points.append(point_text)

    # Generate scale points (using default values)
    scale_points = []
    for i in range(len(points)):
        if i == 0:
            point_text = '(OutVal=(X=1.000000,Y=1.000000,Z=1.000000),InterpMode=CIM_CurveAuto)'
        else:
            point_text = f'(InVal={float(i)},OutVal=(X=1.000000,Y=1.000000,Z=1.000000),InterpMode=CIM_CurveAuto)'
        scale_points.append(point_text)

    # Combine all parts
    spline_text = f'''{header}
            SplineCurves=(Position=(Points=({",".join(position_points)})),Rotation=(Points=({",".join(rotation_points)})),Scale=(Points=({",".join(scale_points)})),ReparamTable=(Points=((),(InVal=1.000000,OutVal=1.000000))))
{footer}'''

    return spline_text


def correct_location(Location):
    # Create a copy of the location to modify
    corrected = Location.copy()
    
    corrected['x'] = (Location['x']*100)-(mapSize*50)
    corrected['y'] = (-Location['z']*100)+(mapSize*50)
    corrected['z'] = (Location['y']*100)

    # Fix Height Offset
    corrected['z'] -= 50000
    
    return corrected

def sub_parent_transform(parentLoc, pointLoc):
    # Subtract parent location from point location
    newx = pointLoc['x'] - parentLoc['x']
    newy = pointLoc['y'] - parentLoc['y'] 
    newz = pointLoc['z'] - parentLoc['z']
    
    return (newx,newy,newz)

try:
    with open(file_path, 'r') as file:
        road_json_data = json.load(file)

    logging.info(f"Loaded JSON data from {file_path}")

    body_text = ''

    for spline in road_json_data['objects']:
        pointList = []
        splineName = spline['name']
        splinePosition = spline['position']
        count = 0
        for point in spline['children']:
            if count == 0: # captureing the first point location as the BP start location
                splinePosition = correct_location(point['position'])
                count += 1
                continue
            if count % 2 != 0:
                count += 1
                continue
            pointPosition = correct_location(point['position'])
            pointList.append(sub_parent_transform(splinePosition,pointPosition))
            print(sub_parent_transform(splinePosition,pointPosition))
            count += 1
        pointList.pop(0)
        prefix = generate_prefix_text(splinePosition)
        
        spline = generate_spline_text(pointList)
        if "road" in splineName.lower():
            # Road spline handling
            suffix = generate_suffix_text('/Game/Environment/roads/asphalt/road_400w_asphaltCleaned.road_400w_asphaltCleaned')
        else:
            # Rail-road spline handling 
            suffix = generate_suffix_text('/Game/Environment/roads/Railroads/train_track_250w.train_track_250w')

        body_text = body_text + prefix + spline + suffix

    full_text = beginText + body_text + endText
    pyperclip.copy(full_text)
    logging.info("Actors copied to clipboard successfully")

except Exception as e:
    logging.error(f"An error occurred: {e}")

input('Press Enter to exit...')

