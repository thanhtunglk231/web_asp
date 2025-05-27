import React, { useState } from 'react';
import { register } from '../services/registerService'; // ??m b?o ???ng d?n ?úng

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
            console.log('??ng ký thành công:', result);
            alert('??ng ký thành công! B?n có th? ??ng nh?p.');
            switchToLogin(); // chuy?n v? form ??ng nh?p n?u c?n
        } catch (error) {
            console.error('L?i ??ng ký:', error.message);
            alert(error.message);
        }
    };

    return (
        <div className="register-container">
            <form onSubmit={handleSubmit} className="register-form">
                <h2>??ng ký</h2>

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
                <button type="submit">??ng ký</button>
                <p>?ã có tài kho?n? <span onClick={switchToLogin} style={{ cursor: 'pointer', color: 'blue' }}>??ng nh?p</span></p>
            </form>
        </div>
    );
};

export default Register;
