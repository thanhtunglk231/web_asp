import React, { useState } from 'react';
import { register } from '../services/registerService'; // ??m b?o ???ng d?n ?�ng

const Register = ({ switchToLogin }) => {
    const [formData, setFormData] = useState({ email: '', password: '' });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const result = await register(formData);
            console.log('??ng k� th�nh c�ng:', result);
            alert('??ng k� th�nh c�ng! B?n c� th? ??ng nh?p.');
            switchToLogin(); // chuy?n v? form ??ng nh?p n?u c?n
        } catch (error) {
            console.error('L?i ??ng k�:', error.message);
            alert(error.message);
        }
    };

    return (
        <div className="register-container">
            <form onSubmit={handleSubmit} className="register-form">
                <h2>??ng k�</h2>

                <input
                    type="email"
                    name="email"
                    placeholder="Email"
                    value={formData.email}
                    onChange={handleChange}
                    required
                />
                <input
                    type="password"
                    name="password"
                    placeholder="M?t kh?u"
                    value={formData.password}
                    onChange={handleChange}
                    required
                />
                <button type="submit">??ng k�</button>
                <p>?� c� t�i kho?n? <span onClick={switchToLogin} style={{ cursor: 'pointer', color: 'blue' }}>??ng nh?p</span></p>
            </form>
        </div>
    );
};

export default Register;
