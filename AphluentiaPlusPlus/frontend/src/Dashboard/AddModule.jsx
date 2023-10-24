import React from 'react';
import Popup from 'reactjs-popup';
import axios from 'axios';
import Button from '@mui/material/Button';
import QRCode from 'react-qr-code';
import './AddModule.css'
import {useContext, useState, useEffect} from 'react';

const AddModule = () => {
    const [applications, setApplications] = useState([]);
    const token = localStorage.getItem('Token');
    const [qrCodeData, setQrCodeData] = useState('');
    const [selectedApp, setSelectedApp] = useState('');
    const [selectedVersion, setSelectedVersion] = useState('');
    const [versions, setVersions] = useState([]);
    const handleAppChange = (event) => {
        const appName = event.target.value;
        setSelectedApp(appName);
        setVersions(applications[appName] || []);
      };
    const handleVersionChange = (event) => {
        const versionId = event.target.value;
        setSelectedVersion(versionId);
        generateQrCode(event.target.value);
    };
    const generateQrCode=(versionId)=>{
        console.log("https://localhost:7176/api/Patient/QrCode/"+selectedApp+"/Version/"+versionId)
        axios
        .get("https://localhost:7176/api/Patient/QrCode/"+selectedApp+"/Version/"+versionId)
        .then(data => {
            setQrCodeData("data:image/jpeg;base64,"+data.data.data)
        })
        .catch(error => {
            // Handle error
    });
    }
    
    const getApplicationData = () => {
        
        axios
            .get("https://localhost:7176/api/Applications")
            .then(data => {
                var fullData = {};
                for (let application of data.data.data){
                    fullData[application.applicationName] = [];
                    for (let version of application.versions){
                        fullData[application.applicationName].push(version.versionId);
                        
                    }
                }
                setApplications(fullData);
            })
            .catch(error => {
                // Handle error
        });
    }
    useEffect(() => {
        getApplicationData();
    }, []);


      
    return  <>
                <div>
                    <select onChange={handleAppChange} value={selectedApp}>
                        <option value=''>Select Application</option>
                        {Object.keys(applications).map(appName => (
                        <option key={appName} value={appName}>
                            {appName}
                        </option>
                        ))}
                    </select>

                    <select onChange={handleVersionChange} value={selectedVersion}>
                        <option  value=''>Select Version</option>
                        {versions.map(version => (
                        <option key={version} value={version}>
                            {version}
                        </option>
                        ))}
                    </select>
                </div>
                <Popup
                trigger={ 
                    <Button variant="outlined" >
                        Generate QR Code
                    </Button>}
                modal
                nested >

                {close => (
                    <div className="modal">
                        <button className="close" onClick={close}>
                        &times;
                        </button>
                        <div className="header"> Pair Existing Application </div>
                        <div className="content">
                            <img
                                src={qrCodeData}
                                alt="Profile"
                                style={{ maxWidth: '300px' }}
                                onClick={(e) => e.preventDefault()}
                            />
                        </div>
                       
                    </div>
                )}
            </Popup>
            </>
          
    }
export default AddModule;