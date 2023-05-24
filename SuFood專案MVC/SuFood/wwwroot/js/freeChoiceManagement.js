const vm = new Vue({
    el: ".Recent-order",
    data: {
        deleteId: undefined,
        editId: undefined,
        toast: "",
        keyword: "",
        sortType: "0",
        selectedStatus: "all",
        selectedCategory: "全部",
        products: [],
        uplodaImgPreview: {
            preview: null,
            image: null,
        },
        EditProdcutItem: {
            ProductId: this.editId,
            ProductName: "蘋果",
            ProductDescription: "好吃喔",
            StockUnit: 200,
            Category: "水果",
            Cost: 20,
            Price: 35,
            StockQuantity: 300,
            Img: null
        },
        EditOrDeleteorCreate: "Edit",
        modalContentStyle: {
            w200: true,
            w800: false
        },
        toastHintStlye: {
            fadeInUp: false
        },
        modalContainerStyle: {
            showModal: false,
        },
        request: {
            productId: 0,
            ProductName: "",
            ProductDescription: "",
            StockUnit: "",
            Category: "",
            Cost: "",
            Price: "",
            StockQuantity: "",
            Img: null
        }
    },
    computed: {
        filterproducts() {
            //搜尋篩選
            arr = this.products.filter(p => {
                return p.productName.indexOf(this.keyword) != -1;
            })
            //用狀態篩選
            switch (this.selectedStatus) {
                case "all":
                    break;
                case "active":
                    arr = this.products.filter(p => {
                        return p.stockQuantity > 100
                    })
                    break;
                case "frozen":
                    arr = this.products.filter(p => {
                        return p.stockQuantity < 100
                    })
                    break;
            }
            //用種類篩選
            switch (this.selectedCategory) {
                case "全部":
                    break;
                case "零食":
                    arr = arr.filter(p => {
                        return p.category.indexOf(this.selectedCategory) != -1;
                    })
                    break;
                case "水果":
                    arr = arr.filter(p => {
                        return p.category.indexOf(this.selectedCategory) !== -1;
                    })
                    break;
                case "蔬菜":
                    arr = arr.filter(p => {
                        return p.category.indexOf(this.selectedCategory) !== -1;
                    })
                //之後要刪掉的
                case "人類":
                    arr = arr.filter(p => {
                        return p.category.indexOf(this.selectedCategory) !== -1;
                    })
                    break;
                default:
                    break;
            }

            ////判斷是否需要排序
            if (this.sorttype) {
                arr.sort((p1, p2) => {
                    return this.sorttype === 1 ? p2.amount - p1.amount : 0;
                })
            }
            return arr
        },
        modaltitle() {
            switch (this.EditOrDeleteorCreate) {
                case "Edit":
                    return "修改商品"
                    console.log("edit")
                    break;
                case "Create":
                    return "新增商品"
                    console.log("edit")
                    break;
                case "Delete":
                    return "刪除商品"
                    console.log("edit")
                    break;
            }
        },
        Categories() {
            return this.products.map(p => p.category).filter(function (element, index, arr) {
                return arr.indexOf(element) == index;
            })
        },
    },
    methods: {
        previewImage: function (event) {
            var input = event.target;
            if (input.files) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    this.uplodaImgPreview.preview = e.target.result;
                }
                this.uplodaImgPreview.image = input.files[0];
                this.request.Img = input.files[0];
                reader.readAsDataURL(input.files[0]);
            }
        },
        openEditModal(item) {
            this.uplodaImgPreview.preview = "data:image/png;base64," + item.img
            this.EditOrDeleteorCreate = "Edit"
            this.modalContentStyle.w200 = false
            this.modalContentStyle.w800 = true
            this.EditProdcutItem = item
            this.editId = item.productId
            this.openModal()
        },
        DeleteProducts(p) {
            this.EditOrDeleteorCreate = "Delete"
            this.modalContentStyle.w200 = true
            this.modalContentStyle.w800 = false
            this.deleteId = p.productId
            this.openModal()
        },
        CreateProducts() {
            this.uplodaImgPreview.preview = ""
            this.EditOrDeleteorCreate = "Create"
            this.modalContentStyle.w200 = false
            this.modalContentStyle.w800 = true
            this.openModal()
        },
        toastHint() {
            this.toastHintStlye.fadeInUp = true
            setTimeout(() => {
                this.toastHintStlye.fadeInUp = false
            }, 2100)
            this.uplodaImgPreview.preview = ""
        },
        openModal() {
            this.modalContainerStyle.showModal = true;
        },
        closeModal() {
            this.modalContainerStyle.showModal = false;
        },
        closeModalWithHint() {
            this.modalContainerStyle.showModal = false;
            this.toastHint();
        },
        clearRequset() {
            this.request =  {
                productId: 0,
                ProductName: "",
                ProductDescription: "",
                StockUnit: "",
                Category: "",
                Cost: "",
                Price: "",
                StockQuantity: "",
                Img: null
           }
        },
        //查詢資料的方法
        GetProductDetail() {
            let _this = this;
            axios.get("/BackStage/FreeChoiceProductManagement/GetProducts").then(response => {
                this.products = response.data
                let productsList = []
                for (var i = 0; i < _this.products.length; i++) {
                    var item = _this.products[i];
                    item.icon = "bx bxs-edit tableIcon";
                    item.trash = "bx bx-trash tableIcon";
                    productsList.push(item);
                }
                _this.products = productsList
            })
        },
        //刪除資料的方法
        DeleteProduct(id) {
            let _this = this;
            axios.delete(`/BackStage/FreeChoiceProductManagement/DeleteProducts/${id}`) //`https://localhost:7009/api/Products/${id}`
                .then(response => {
                    this.toast = response.data
                    this.closeModalWithHint()
                    _this.GetProductDetail()
                })
        },
        //編輯資料的方法
        EditProduct() {
            let _this = this;
            let item = _this.EditProdcutItem;

            const formData = new FormData();
            formData.append('ProductId', this.editId);
            formData.append('ProductName', item.productName);
            formData.append('ProductDescription', item.productDescription);
            formData.append('StockUnit', item.stockUnit);
            formData.append('Category', item.category);
            formData.append('Cost', item.cost);
            formData.append('Price', item.price);
            formData.append('StockQuantity', item.stockQuantity);
            formData.append('Img', this.uplodaImgPreview.image);

            axios.post("/BackStage/FreeChoiceProductManagement/Edit", formData, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    this.toast = response.data
                    this.closeModalWithHint()
                    _this.GetProductDetail()
                })
                .catch((error) => console.log("錯誤", this.toast = response.data))
        },
        //新增資料的方法
        CreateProduct() {
            let _this = this;
            axios.post("/BackStage/FreeChoiceProductManagement/Create", this.request, { headers: { 'Content-Type': 'multipart/form-data' } })
                .then(response => {
                    _this.clearRequset()
                    this.toast = response.data
                    this.closeModalWithHint()
                    _this.GetProductDetail()
                })
                .catch((error) => { console.error(error) })
        },
    },
    mounted() {
        this.GetProductDetail();
    }
})