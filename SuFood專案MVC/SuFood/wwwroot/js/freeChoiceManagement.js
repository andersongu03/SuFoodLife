const vm = new Vue({
    el: ".Recent-order",
    data: {
        isNowEditHelp: false,
        tobeEditedHelpList:[],
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
        editImg:"",
        EditProdcutItem: {
            ProductId: this.editId,
            ProductName: "",
            ProductDescription: "",
            StockUnit: 0,
            Category: "",
            Cost: 0,
            Price: 0,
            StockQuantity: 0,
            Img: null
        },
        OldEditProductItem: {},
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
        },
        warmMessage: {
            errProductName: false,
            errProductDescription: false,
            errStockUnit: false,
            errCategory: false,
            errCost: false,
            errPrice: false,
            errorStockQuantity: false,
            error: false,
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
        EditPreviewImg() {
            return this.uplodaImgPreview.preview == '' ? this.editImg : this.uplodaImgPreview.preview
        }
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
            this.editImg = "data:image/png;base64," + item.img
            this.uplodaImgPreview.preview = "data:image/png;base64," + item.img
            this.EditOrDeleteorCreate = "Edit"
            this.modalContentStyle.w200 = false
            this.modalContentStyle.w800 = true
            this.EditProdcutItem = item
            this.editId = item.productId
            this.openModal()
            this.storeOldEditData(item)
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
            this.reverseEditData()
        },
        closeModalWithHint() {
            this.modalContainerStyle.showModal = false;
            this.toastHint();
        },
        storeOldEditData(item) {
            this.OldEditProductItem.productName = item.productName
            this.OldEditProductItem.category = item.category
            this.OldEditProductItem.cost = item.cost
            this.OldEditProductItem.img = item.img
            this.OldEditProductItem.price = item.price
            this.OldEditProductItem.productDescription = item.productDescription
            this.OldEditProductItem.quantity = item.quantity
            this.OldEditProductItem.stockUnit = item.stockUnit
            this.OldEditProductItem.stockQuantity = item.stockQuantity
        },
        reverseEditData() {
            let target = vm.products.find(x => x.productId == this.editId)
            target.productName = this.OldEditProductItem.productName
            target.category = this.OldEditProductItem.category
            target.cost = this.OldEditProductItem.cost 
            target.img = this.OldEditProductItem.img 
            target.price = this.OldEditProductItem.price
            target.productDescription = this.OldEditProductItem.productDescription
            target.quantity = this.OldEditProductItem.quantity
            target.stockUnit = this.OldEditProductItem.stockUni
            target.stockQuantity = this.OldEditProductItem.stockQuantity
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
            if (item.productDescription != '' && item.category != '' && item.cost != '' && item.price != '' && item.productDescription != '' && item.productName != '' && item.stockQuantity != '' && item.stockUnit != '') {
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
                axios.post("/BackStage/FreeChoiceProductManagement/Edit", formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                    .then(response => {
                        this.closeModalWithHint()
                        this.toast = response.data
                        _this.GetProductDetail()
                    }).catch((error) => console.log(error))
            }
            this.toast = "請確認內容";
            this.toastHint();
            this.uplodaImgPreview.image = null
        },
        CreateProduct() {
            if (this.CreateRequestIsNotEmpty()) {
                let _this = this;
                axios.post("/BackStage/FreeChoiceProductManagement/Create", this.request, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                    .then(response => {
                        this.toast = response.data
                        this.closeModalWithHint()
                        _this.GetProductDetail()
                        _this.clearRquest()
                    })
            }
            this.toast = "請確認內容";
            this.toastHint();
        },
        CreateRequestIsNotEmpty() {
            let r = this.request;
            return (r.Category == '' && r.Cost == '' && r.Price == '' && r.ProductDescription == '' && r.StockQuantity == '' && r.StockUnit == '' && r.ProductName == '') ? false : true;
        },
        EditRequestIsNotEmpty() {
            let r = this.EditProdcutItem;
            return (r.category == '' && r.cost == '' && r.price == '' && r.productDescription == '' && r.stockQuantity == '' && r.stockUnit == '' && r.productName == '') ? false : true;
        },
        clearRquest() {
            this.request = {
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
        EditHelp() {
            this.isNowEditHelp = true;
        },
        calcelHelp() {
            this.isNowEditHelp = false;
        },
        submitHelp() {
            this.isNowEditHelp = false;
            let result = this.products.filter(a => a.isHelpUchioce == true)

            axios.post("/BackStage/FreeChoiceProductManagement/EditHelpChoiceList", result)
                .then(response => {
                    console.log(response)
            })
        }
    },

    mounted() {
        this.GetProductDetail();
    },
})