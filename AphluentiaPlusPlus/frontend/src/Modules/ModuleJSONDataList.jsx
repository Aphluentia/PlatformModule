import React from 'react';
import './ModuleJSONDataList.css';
import ModuleJSONData from './ModuleJSONData';

const ModuleJSONDataList = ({module, handleDataChange, setModule}) => {
    
    return (
        module === null 
        ? null 
        : ( <>
            <p className='module-section-title'><b>Module Data</b></p>
                <div className="module-data-structure">
                    {module.moduleData.dataStructure.map((datapoint, index) => {
                        if (datapoint.contextName === module.moduleData.activeContextName) 
                            return (
                                <ModuleJSONData 
                                    key={index} 
                                    initialDatapoint={datapoint} 
                                    handleDataChange={handleDataChange} 
                                    setModule={setModule}
                                />
                            );
                    })}
                </div>
            </>)
        )
    };

export default ModuleJSONDataList;
