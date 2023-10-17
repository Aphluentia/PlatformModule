
import axios from 'axios';

import React, { useState, useEffect } from 'react';

import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';
import Button from '@mui/material/Button';

const ApplicationCode = () => {
  
  const [moduleId, setModuleId] = useState('');
  const token = localStorage.getItem('Authentication');
  
  useEffect(() => {
    const email = localStorage.getItem('Email');
   
  }, []);
  const changed = (value) => {
    setModuleId(value);
  };

  const handleFieldClick = (email) => {
    console.log(email);
    axios
    .post("https://localhost:7176/api/Patient/" + token + "/" + email)
    .then(data => {
      getPatientTherapists();
    })
    .catch(error => {
      // Handle error
    });
  };


  return (
    <div>
         <input 
              type="text"
              value={moduleId}
              onChange={changed}
            />
            <Button  variant="contained"
              startIcon={<SaveOutlinedIcon />} 
              onClick={() => handleFieldClick(null)}>Save</Button>
    </div>
  );
};

export default ApplicationCode;
