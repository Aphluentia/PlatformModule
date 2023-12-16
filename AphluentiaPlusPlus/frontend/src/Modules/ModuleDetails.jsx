import React from 'react';
import './ModuleDetails.css';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import { InfinitySpin } from  'react-loader-spinner'

const ModuleDetails = ({module, updateToVersion}) => {
    const [selectedVersion, setSelectedVersion] = useState('');
    const [versions, setVersions] = useState([]);
    const [loading, setLoading] = useState(module == null);
   
    const update = () => {
        setLoading(true);
        if (selectedVersion !== '') {
            updateToVersion(selectedVersion);
            setLoading(false);
        }
        
    }
    const getApplicationVersions = () => {
        setLoading(true);
        axios
            .get("https://localhost:7176/api/Applications/"+module.moduleData.applicationName)
            .then(data => {
                let temp = []
                for (let version of data.data.data.versions){
                    temp.push(version.versionId);  
                }
                setVersions(temp);
                setLoading(false);
            })
            .catch(error => {
                // Handle error
        });
    }
    useEffect(() => {
        getApplicationVersions();
    }, []);
    const changeModuleVersion = (event) => {
        setLoading(true);
        const versionId = event.target.value;
        setSelectedVersion(versionId);
        setLoading(false);
    };
    return (
        loading == true
        ?  <InfinitySpin 
                color="#4fa94d"
                height={170}
                width={370}
            />  
        : ( <>
                <div className='module-version-container'>
                    <h1 className="module-title">Application: {module.moduleData.applicationName}</h1>
                    <p>Module Identifier: {module.id}</p>

                    <p className="module-version">
                        <span>Version: {module.moduleData.versionId}</span>
                        <select className="version-select" onChange={changeModuleVersion} value={selectedVersion}>
                            <option value=''>Select Version</option>
                            {versions.map(version => (
                                <option key={version} value={version}>
                                    {version}
                                </option>
                            ))}
                        </select>
                        <input 
                            className="update-version-button"
                            type="button" 
                            value="Update To Version"
                            onClick={update} 
                        />
                    </p>
                </div>
            </>)
        )
    };

export default ModuleDetails;
